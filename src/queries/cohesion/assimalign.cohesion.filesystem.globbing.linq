<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>Assimalign.Cohesion.FileSystem.Globbing.PathSegments</Namespace>
<Namespace>Assimalign.Cohesion.FileSystem.Globbing.Internal.Utilities</Namespace>
<Namespace>Assimalign.Cohesion.FileSystem.Globbing.PatternContexts</Namespace>
<Namespace>Assimalign.Cohesion.FileSystem.Globbing.Internal</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.cohesion.filesystem"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.FileSystem.Globbing(net8.0)
namespace Assimalign.Cohesion.FileSystem.Globbing
{
	#region \
	public class FileMatcherContext
	{
	    private readonly IFileSystemDirectory _root;
	    private readonly List<IFilePatternContext> _includePatternContexts;
	    private readonly List<IFilePatternContext> _excludePatternContexts;
	    private readonly List<FilePatternMatch> _files;
	    private readonly HashSet<string> _declaredLiteralFolderSegmentInString;
	    private readonly HashSet<FilePathLiteralSegment> _declaredLiteralFolderSegments = new HashSet<FilePathLiteralSegment>();
	    private readonly HashSet<FilePathLiteralSegment> _declaredLiteralFileSegments = new HashSet<FilePathLiteralSegment>();
	    private bool _declaredParentPathSegment;
	    private bool _declaredWildcardPathSegment;
	    private readonly StringComparison _comparisonType;
	    public FileMatcherContext(
	        IEnumerable<IFilePattern> includePatterns,
	        IEnumerable<IFilePattern> excludePatterns,
	        IFileSystemDirectory container,
	        StringComparison comparison)
	    {
	        _root = container;
	        _files = new List<FilePatternMatch>();
	        _comparisonType = comparison;
	        _includePatternContexts = includePatterns.Select(pattern => pattern.CreatePatternContextForInclude()).ToList();
	        _excludePatternContexts = excludePatterns.Select(pattern => pattern.CreatePatternContextForExclude()).ToList();
	        _declaredLiteralFolderSegmentInString = new HashSet<string>(StringComparisonHelper.GetStringComparer(comparison));
	    }
	    public FilePatternMatchingResult Execute()
	    {
	        _files.Clear();
	        Match(_root, parentRelativePath: null);
	        return new FilePatternMatchingResult(_files, _files.Count > 0);
	    }
	    private void Match(IFileSystemDirectory directory, string parentRelativePath)
	    {
	        // Request all the including and excluding patterns to push current directory onto their status stack.
	        PushDirectory(directory);
	        Declare();
	        var entities = new List<IFileSystemInfo>();
	        if (_declaredWildcardPathSegment || _declaredLiteralFileSegments.Any())
	        {
	            entities.AddRange(directory);
	        }
	        else
	        {
	            foreach (var candidate in directory)
	            {
	                if (_declaredLiteralFolderSegmentInString.Contains(candidate.Name))
	                {
	                    entities.Add(candidate);
	                }
	            }
	        }
	        if (_declaredParentPathSegment)
	        {
	            entities.Add(directory.GetDirectory(".."));
	        }
	        // collect files and sub directories
	        var subDirectories = new List<IFileSystemDirectory>();
	        foreach (var fileInfo in entities)
	        {
	            if (fileInfo is IFileSystemDirectory directoryInfo)
	            {
	                if (MatchPatternContexts(directoryInfo, (pattern, dir) => pattern.Test(dir)))
	                {
	                    subDirectories.Add(directoryInfo);
	                }
	                continue;
	            }
	            else 
	            {
	                FilePatternTestResult result = MatchPatternContexts(fileInfo, (pattern, file) => pattern.Test(file));
	                if (result.IsSuccessful)
	                {
	                    _files.Add(new FilePatternMatch(
	                        path: CombinePath(parentRelativePath, fileInfo.Name),
	                        stem: result.Stem));
	                }
	                continue;
	            }
	        }
	        // Matches the sub directories recursively
	        foreach (var subDir in subDirectories)
	        {
	            string relativePath = CombinePath(parentRelativePath, subDir.Name);
	            Match(subDir, relativePath);
	        }
	        // Request all the including and excluding patterns to pop their status stack.
	        PopDirectory();
	    }
	    private void Declare()
	    {
	        _declaredLiteralFileSegments.Clear();
	        _declaredLiteralFolderSegments.Clear();
	        _declaredParentPathSegment = false;
	        _declaredWildcardPathSegment = false;
	        foreach (IFilePatternContext include in _includePatternContexts)
	        {
	            include.Declare(DeclareInclude);
	        }
	    }
	    private void DeclareInclude(IFilePathSegment patternSegment, bool isLastSegment)
	    {
	        var literalSegment = patternSegment as FilePathLiteralSegment;
	        if (literalSegment != null)
	        {
	            if (isLastSegment)
	            {
	                _declaredLiteralFileSegments.Add(literalSegment);
	            }
	            else
	            {
	                _declaredLiteralFolderSegments.Add(literalSegment);
	                _declaredLiteralFolderSegmentInString.Add(literalSegment.Value);
	            }
	        }
	        else if (patternSegment is FilePathParentSegment)
	        {
	            _declaredParentPathSegment = true;
	        }
	        else if (patternSegment is FilePathWildcardSegment)
	        {
	            _declaredWildcardPathSegment = true;
	        }
	    }
	    internal static string CombinePath(string left, string right)
	    {
	        if (string.IsNullOrEmpty(left))
	        {
	            return right;
	        }
	        else
	        {
	            return $"{left}/{right}";
	        }
	    }
	    // Used to adapt Test(DirectoryInfoBase) for the below overload
	    private bool MatchPatternContexts<TFileInfoBase>(TFileInfoBase fileinfo, Func<IFilePatternContext, TFileInfoBase, bool> test)
	    {
	        return MatchPatternContexts(
	            fileinfo,
	            (ctx, file) =>
	            {
	                if (test(ctx, file))
	                {
	                    return FilePatternTestResult.Success(stem: string.Empty);
	                }
	                else
	                {
	                    return FilePatternTestResult.Failed;
	                }
	            }).IsSuccessful;
	    }
	    private FilePatternTestResult MatchPatternContexts<TFileInfoBase>(TFileInfoBase fileinfo, Func<IFilePatternContext, TFileInfoBase, FilePatternTestResult> test)
	    {
	        FilePatternTestResult result = FilePatternTestResult.Failed;
	        // If the given file/directory matches any including pattern, continues to next step.
	        foreach (IFilePatternContext context in _includePatternContexts)
	        {
	            FilePatternTestResult localResult = test(context, fileinfo);
	            if (localResult.IsSuccessful)
	            {
	                result = localResult;
	                break;
	            }
	        }
	        // If the given file/directory doesn't match any of the including pattern, returns false.
	        if (!result.IsSuccessful)
	        {
	            return FilePatternTestResult.Failed;
	        }
	        // If the given file/directory matches any excluding pattern, returns false.
	        foreach (IFilePatternContext context in _excludePatternContexts)
	        {
	            if (test(context, fileinfo).IsSuccessful)
	            {
	                return FilePatternTestResult.Failed;
	            }
	        }
	        return result;
	    }
	    private void PopDirectory()
	    {
	        foreach (IFilePatternContext context in _excludePatternContexts)
	        {
	            context.PopDirectory();
	        }
	        foreach (IFilePatternContext context in _includePatternContexts)
	        {
	            context.PopDirectory();
	        }
	    }
	    private void PushDirectory(IFileSystemDirectory directory)
	    {
	        foreach (IFilePatternContext context in _includePatternContexts)
	        {
	            context.PushDirectory(directory);
	        }
	        foreach (IFilePatternContext context in _excludePatternContexts)
	        {
	            context.PushDirectory(directory);
	        }
	    }
	}
	public class FilePatternBuilder
	{
	    private static readonly char[] slashes = new[] { '/', '\\' };
	    private static readonly char[] star = new[] { '*' };
	    public FilePatternBuilder()
	    {
	        ComparisonType = StringComparison.OrdinalIgnoreCase;
	    }
	    public FilePatternBuilder(StringComparison comparisonType)
	    {
	        ComparisonType = comparisonType;
	    }
	    public StringComparison ComparisonType { get; }
	    public IFilePattern Build(string pattern)
	    {
	        if (pattern == null)
	        {
	            throw new ArgumentNullException(nameof(pattern));
	        }
	        pattern = pattern.TrimStart(slashes);
	        if (pattern.TrimEnd(slashes).Length < pattern.Length)
	        {
	            // If the pattern end with a slash, it is considered a
	            // a directory.
	            pattern = pattern.TrimEnd(slashes) + "/**";
	        }
	        var allSegments = new List<IFilePathSegment>();
	        var isParentSegmentLegal = true;
	        IList<IFilePathSegment> segmentsPatternStartsWith = null;
	        IList<IList<IFilePathSegment>> segmentsPatternContains = null;
	        IList<IFilePathSegment> segmentsPatternEndsWith = null;
	        var endPattern = pattern.Length;
	        for (var scanPattern = 0; scanPattern < endPattern;)
	        {
	            var beginSegment = scanPattern;
	            var endSegment = NextIndex(pattern, slashes, scanPattern, endPattern);
	            IFilePathSegment segment = null;
	            if (segment == null && endSegment - beginSegment == 3)
	            {
	                if (pattern[beginSegment] == '*' &&
	                    pattern[beginSegment + 1] == '.' &&
	                    pattern[beginSegment + 2] == '*')
	                {
	                    // turn *.* into *
	                    beginSegment += 2;
	                }
	            }
	            if (segment == null && endSegment - beginSegment == 2)
	            {
	                if (pattern[beginSegment] == '*' &&
	                    pattern[beginSegment + 1] == '*')
	                {
	                    // recognized **
	                    segment = new RecursiveWildcardSegment();
	                }
	                else if (pattern[beginSegment] == '.' &&
	                         pattern[beginSegment + 1] == '.')
	                {
	                    // recognized ..
	                    if (!isParentSegmentLegal)
	                    {
	                        throw new ArgumentException("\"..\" can be only added at the beginning of the pattern.");
	                    }
	                    segment = new FilePathParentSegment();
	                }
	            }
	            if (segment == null && endSegment - beginSegment == 1)
	            {
	                if (pattern[beginSegment] == '.')
	                {
	                    // recognized .
	                    segment = new FilePathCurrentSegment();
	                }
	            }
	            if (segment == null && endSegment - beginSegment > 2)
	            {
	                if (pattern[beginSegment] == '*' &&
	                    pattern[beginSegment + 1] == '*' &&
	                    pattern[beginSegment + 2] == '.')
	                {
	                    // recognize **.
	                    // swallow the first *, add the recursive path segment and
	                    // the remaining part will be treat as wild card in next loop.
	                    segment = new RecursiveWildcardSegment();
	                    endSegment = beginSegment;
	                }
	            }
	            if (segment == null)
	            {
	                string beginsWith = string.Empty;
	                var contains = new List<string>();
	                string endsWith = string.Empty;
	                for (int scanSegment = beginSegment; scanSegment < endSegment;)
	                {
	                    int beginLiteral = scanSegment;
	                    int endLiteral = NextIndex(pattern, star, scanSegment, endSegment);
	                    if (beginLiteral == beginSegment)
	                    {
	                        if (endLiteral == endSegment)
	                        {
	                            // and the only bit
	                            segment = new FilePathLiteralSegment(Portion(pattern, beginLiteral, endLiteral), ComparisonType);
	                        }
	                        else
	                        {
	                            // this is the first bit
	                            beginsWith = Portion(pattern, beginLiteral, endLiteral);
	                        }
	                    }
	                    else if (endLiteral == endSegment)
	                    {
	                        // this is the last bit
	                        endsWith = Portion(pattern, beginLiteral, endLiteral);
	                    }
	                    else
	                    {
	                        if (beginLiteral != endLiteral)
	                        {
	                            // this is a middle bit
	                            contains.Add(Portion(pattern, beginLiteral, endLiteral));
	                        }
	                        else
	                        {
	                            // note: NOOP here, adjacent *'s are collapsed when they
	                            // are mixed with literal text in a path segment
	                        }
	                    }
	                    scanSegment = endLiteral + 1;
	                }
	                if (segment == null)
	                {
	                    segment = new FilePathWildcardSegment(beginsWith, contains, endsWith, ComparisonType);
	                }
	            }
	            if (segment is not FilePathParentSegment)
	            {
	                isParentSegmentLegal = false;
	            }
	            if (segment is FilePathCurrentSegment)
	            {
	                // ignore ".\"
	            }
	            else
	            {
	                if (segment is RecursiveWildcardSegment)
	                {
	                    if (segmentsPatternStartsWith == null)
	                    {
	                        segmentsPatternStartsWith = new List<IFilePathSegment>(allSegments);
	                        segmentsPatternEndsWith = new List<IFilePathSegment>();
	                        segmentsPatternContains = new List<IList<IFilePathSegment>>();
	                    }
	                    else if (segmentsPatternEndsWith.Count != 0)
	                    {
	                        segmentsPatternContains.Add(segmentsPatternEndsWith);
	                        segmentsPatternEndsWith = new List<IFilePathSegment>();
	                    }
	                }
	                else if (segmentsPatternEndsWith != null)
	                {
	                    segmentsPatternEndsWith.Add(segment);
	                }
	                allSegments.Add(segment);
	            }
	            scanPattern = endSegment + 1;
	        }
	        if (segmentsPatternStartsWith == null)
	        {
	            return new LinearPattern(allSegments);
	        }
	        else
	        {
	            return new RaggedPattern(allSegments, segmentsPatternStartsWith, segmentsPatternEndsWith, segmentsPatternContains);
	        }
	    }
	    private static int NextIndex(string pattern, char[] anyOf, int beginIndex, int endIndex)
	    {
	        int index = pattern.IndexOfAny(anyOf, beginIndex, endIndex - beginIndex);
	        return index == -1 ? endIndex : index;
	    }
	    private static string Portion(string pattern, int beginIndex, int endIndex)
	    {
	        return pattern.Substring(beginIndex, endIndex - beginIndex);
	    }
	    private sealed class LinearPattern : IFileLinearPattern
	    {
	        public LinearPattern(List<IFilePathSegment> allSegments)
	        {
	            Segments = allSegments;
	        }
	        public IList<IFilePathSegment> Segments { get; }
	        public IFilePatternContext CreatePatternContextForInclude()
	        {
	            return new FilePatternContextLinearInclude(this);
	        }
	        public IFilePatternContext CreatePatternContextForExclude()
	        {
	            return new FilePatternContextLinearExclude(this);
	        }
	    }
	    private sealed class RaggedPattern : IFileRaggedPattern
	    {
	        public RaggedPattern(List<IFilePathSegment> allSegments, IList<IFilePathSegment> segmentsPatternStartsWith, IList<IFilePathSegment> segmentsPatternEndsWith, IList<IList<IFilePathSegment>> segmentsPatternContains)
	        {
	            Segments = allSegments;
	            StartsWith = segmentsPatternStartsWith;
	            Contains = segmentsPatternContains;
	            EndsWith = segmentsPatternEndsWith;
	        }
	        public IList<IList<IFilePathSegment>> Contains { get; }
	        public IList<IFilePathSegment> EndsWith { get; }
	        public IList<IFilePathSegment> Segments { get; }
	        public IList<IFilePathSegment> StartsWith { get; }
	        public IFilePatternContext CreatePatternContextForInclude()
	        {
	            return new FilePatternContextRaggedInclude(this);
	        }
	        public IFilePatternContext CreatePatternContextForExclude()
	        {
	            return new FilePatternContextRaggedExclude(this);
	        }
	    }
	}
	public struct FilePatternMatch : IEquatable<FilePatternMatch>
	{
	    public string Path { get; }
	    public string Stem { get; }
	    public FilePatternMatch(string path, string stem)
	    {
	        Path = path;
	        Stem = stem;
	    }
	    public bool Equals(FilePatternMatch other)
	    {
	        return string.Equals(other.Path, Path, StringComparison.OrdinalIgnoreCase) &&
	               string.Equals(other.Stem, Stem, StringComparison.OrdinalIgnoreCase);
	    }
	    public override bool Equals(object obj)
	    {
	        return Equals((FilePatternMatch)obj);
	    }
	    public override int GetHashCode() =>
	        HashHelpers.Combine(GetHashCode(Path), GetHashCode(Stem));
	    private static int GetHashCode(string value) =>
	        value != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(value) : 0;
	}
	public class FilePatternMatcher
	{
	    private readonly IList<IFilePattern> _includePatterns = new List<IFilePattern>();
	    private readonly IList<IFilePattern> _excludePatterns = new List<IFilePattern>();
	    private readonly FilePatternBuilder _builder;
	    private readonly StringComparison _comparison;
	    public FilePatternMatcher() : this(StringComparison.OrdinalIgnoreCase)
	    {
	    }
	    public FilePatternMatcher(StringComparison comparisonType)
	    {
	        _comparison = comparisonType;
	        _builder = new FilePatternBuilder(comparisonType);
	    }
	    public virtual FilePatternMatcher AddInclude(string pattern)
	    {
	        _includePatterns.Add(_builder.Build(pattern));
	        return this;
	    }
	    public virtual FilePatternMatcher AddExclude(string pattern)
	    {
	        _excludePatterns.Add(_builder.Build(pattern));
	        return this;
	    }
	    public virtual FilePatternMatchingResult Execute(IFileSystemDirectory directory)
	    {
	        var context = new FileMatcherContext(_includePatterns, _excludePatterns, directory, _comparison);
	        return context.Execute();
	    }
	}
	public class FilePatternMatchingResult
	{
	    public FilePatternMatchingResult(IEnumerable<FilePatternMatch> files)
	        : this(files, hasMatches: files.Any())
	    {
	        Files = files;
	    }
	    public FilePatternMatchingResult(IEnumerable<FilePatternMatch> files, bool hasMatches)
	    {
	        Files = files;
	        HasMatches = hasMatches;
	    }
	    public IEnumerable<FilePatternMatch> Files { get; set; }
	    public bool HasMatches { get; }
	}
	public struct FilePatternTestResult
	{
	    public static readonly FilePatternTestResult Failed = new FilePatternTestResult(isSuccessful: false, stem: null);
	    public bool IsSuccessful { get; }
	    public string Stem { get; }
	    private FilePatternTestResult(bool isSuccessful, string stem)
	    {
	        IsSuccessful = isSuccessful;
	        Stem = stem;
	    }
	    public static FilePatternTestResult Success(string stem)
	    {
	        return new FilePatternTestResult(isSuccessful: true, stem: stem);
	    }
	}
	#endregion
	#region \Abstractions
	public interface IFileLinearPattern : IFilePattern
	{
	    IList<IFilePathSegment> Segments { get; }
	}
	public interface IFilePathSegment
	{
	    bool CanProduceStem { get; }
	    bool Match(string value);
	}
	public interface IFilePattern
	{
	    IFilePatternContext CreatePatternContextForInclude();
	    IFilePatternContext CreatePatternContextForExclude();
	}
	public interface IFilePatternContext
	{
	    void Declare(Action<IFilePathSegment, bool> onDeclare);
	    bool Test(IFileSystemDirectory directory);
	    FilePatternTestResult Test(IFileSystemFile file);
	    void PushDirectory(IFileSystemDirectory directory);
	    void PopDirectory();
	}
	public interface IFileRaggedPattern : IFilePattern
	{
	    IList<IFilePathSegment> Segments { get; }
	    IList<IFilePathSegment> StartsWith { get; }
	    IList<IList<IFilePathSegment>> Contains { get; }
	    IList<IFilePathSegment> EndsWith { get; }
	}
	#endregion
	#region \Extensions
	public static class FileMatcherExtensions
	{
	    public static void AddExcludePatterns(this FilePatternMatcher matcher, params IEnumerable<string>[] excludePatternsGroups)
	    {
	        foreach (IEnumerable<string> group in excludePatternsGroups)
	        {
	            foreach (string pattern in group)
	            {
	                matcher.AddExclude(pattern);
	            }
	        }
	    }
	    public static void AddIncludePatterns(this FilePatternMatcher matcher, params IEnumerable<string>[] includePatternsGroups)
	    {
	        foreach (IEnumerable<string> group in includePatternsGroups)
	        {
	            foreach (string pattern in group)
	            {
	                matcher.AddInclude(pattern);
	            }
	        }
	    }
	    //public static IEnumerable<string> GetResultsInFullPath(this FilePatternMatcher matcher, string directoryPath)
	    //{
	    //    IEnumerable<FilePatternMatch> matches = matcher.Execute(new FileDirectoryInfo(new DirectoryInfo(directoryPath))).Files;
	    //    string[] result = matches.Select(match => Path.GetFullPath(Path.Combine(directoryPath, match.Path))).ToArray();
	    //    return result;
	    //}
	    //public static FilePatternMatchingResult Match(this FilePatternMatcher matcher, string file)
	    //{
	    //    return Match(matcher, Directory.GetCurrentDirectory(), new List<string> { file });
	    //}
	    //public static FilePatternMatchingResult Match(this FilePatternMatcher matcher, string rootDir, string file)
	    //{
	    //    return Match(matcher, rootDir, new List<string> { file });
	    //}
	    //public static FilePatternMatchingResult Match(this FilePatternMatcher matcher, IEnumerable<string> files)
	    //{
	    //    return Match(matcher, Directory.GetCurrentDirectory(), files);
	    //}
	    //public static FilePatternMatchingResult Match(this FilePatternMatcher matcher, string rootDir, IEnumerable<string> files)
	    //{
	    //    if (matcher == null)
	    //    {
	    //        throw new ArgumentNullException(nameof(matcher));
	    //    }
	    //    return matcher.Execute(new InMemoryFileDirectoryInfo(rootDir, files));
	    //}
	}
	#endregion
	#region \Internal\Utilities
	internal static class HashHelpers
	{
	    public static int Combine(int h1, int h2)
	    {
	        // RyuJIT optimizes this to use the ROL instruction
	        // Related GitHub pull request: https://github.com/dotnet/coreclr/pull/1830
	        uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
	        return ((int)rol5 + h1) ^ h2;
	    }
	}
	internal static class StringComparisonHelper
	{
	    public static StringComparer GetStringComparer(StringComparison comparisonType)
	    {
	        switch (comparisonType)
	        {
	            case StringComparison.CurrentCulture:
	                return StringComparer.CurrentCulture;
	            case StringComparison.CurrentCultureIgnoreCase:
	                return StringComparer.CurrentCultureIgnoreCase;
	            case StringComparison.Ordinal:
	                return StringComparer.Ordinal;
	            case StringComparison.OrdinalIgnoreCase:
	                return StringComparer.OrdinalIgnoreCase;
	            case StringComparison.InvariantCulture:
	                return StringComparer.InvariantCulture;
	            case StringComparison.InvariantCultureIgnoreCase:
	                return StringComparer.InvariantCultureIgnoreCase;
	            default:
	                throw new InvalidOperationException();// SR.Format(SR.UnexpectedStringComparisonType, comparisonType));
	        }
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \PathSegments
	public class FilePathCurrentSegment : IFilePathSegment
	{
	    public bool CanProduceStem { get { return false; } }
	    public bool Match(string value)
	    {
	        return false;
	    }
	}
	public class FilePathLiteralSegment : IFilePathSegment
	{
	    private readonly StringComparison _comparisonType;
	    public bool CanProduceStem { get { return false; } }
	    public FilePathLiteralSegment(string value, StringComparison comparisonType)
	    {
	        if (value == null)
	        {
	            throw new ArgumentNullException(nameof(value));
	        }
	        Value = value;
	        _comparisonType = comparisonType;
	    }
	    public string Value { get; }
	    public bool Match(string value)
	    {
	        return string.Equals(Value, value, _comparisonType);
	    }
	    public override bool Equals(object obj)
	    {
	        var other = obj as FilePathLiteralSegment;
	        return other != null &&
	            _comparisonType == other._comparisonType &&
	            string.Equals(other.Value, Value, _comparisonType);
	    }
	    public override int GetHashCode()
	    {
	        return StringComparisonHelper.GetStringComparer(_comparisonType).GetHashCode(Value);
	    }
	}
	public class FilePathParentSegment : IFilePathSegment
	{
	    private const string LiteralParent = "..";
	    public bool CanProduceStem { get { return false; } }
	    public bool Match(string value)
	    {
	        return string.Equals(LiteralParent, value, StringComparison.Ordinal);
	    }
	}
	public class FilePathWildcardSegment : IFilePathSegment
	{
	    // It doesn't matter which StringComparison type is used in this MatchAll segment because
	    // all comparing are skipped since there is no content in the segment.
	    public static readonly FilePathWildcardSegment MatchAll = new FilePathWildcardSegment(
	        string.Empty, new List<string>(), string.Empty, StringComparison.OrdinalIgnoreCase);
	    private readonly StringComparison _comparisonType;
	    public FilePathWildcardSegment(string beginsWith, List<string> contains, string endsWith, StringComparison comparisonType)
	    {
	        BeginsWith = beginsWith;
	        Contains = contains;
	        EndsWith = endsWith;
	        _comparisonType = comparisonType;
	    }
	    public bool CanProduceStem { get { return true; } }
	    public string BeginsWith { get; }
	    public List<string> Contains { get; }
	    public string EndsWith { get; }
	    public bool Match(string value)
	    {
	        FilePathWildcardSegment wildcard = this;
	        if (value.Length < wildcard.BeginsWith.Length + wildcard.EndsWith.Length)
	        {
	            return false;
	        }
	        if (!value.StartsWith(wildcard.BeginsWith, _comparisonType))
	        {
	            return false;
	        }
	        if (!value.EndsWith(wildcard.EndsWith, _comparisonType))
	        {
	            return false;
	        }
	        int beginRemaining = wildcard.BeginsWith.Length;
	        int endRemaining = value.Length - wildcard.EndsWith.Length;
	        for (int containsIndex = 0; containsIndex != wildcard.Contains.Count; ++containsIndex)
	        {
	            string containsValue = wildcard.Contains[containsIndex];
	            int indexOf = value.IndexOf(
	                value: containsValue,
	                startIndex: beginRemaining,
	                count: endRemaining - beginRemaining,
	                comparisonType: _comparisonType);
	            if (indexOf == -1)
	            {
	                return false;
	            }
	            beginRemaining = indexOf + containsValue.Length;
	        }
	        return true;
	    }
	}
	public class RecursiveWildcardSegment : IFilePathSegment
	{
	    public bool CanProduceStem { get { return true; } }
	    public bool Match(string value)
	    {
	        return false;
	    }
	}
	#endregion
	#region \PatternContexts
	public abstract class FilePatternContext<TFrame> : IFilePatternContext
	{
	    private Stack<TFrame> _stack = new Stack<TFrame>();
	    protected TFrame Frame;
	    public virtual void Declare(Action<IFilePathSegment, bool> declare) { }
	    public abstract FilePatternTestResult Test(IFileSystemFile file);
	    public abstract bool Test(IFileSystemDirectory directory);
	    public abstract void PushDirectory(IFileSystemDirectory directory);
	    public virtual void PopDirectory()
	    {
	        Frame = _stack.Pop();
	    }
	    protected void PushDataFrame(TFrame frame)
	    {
	        _stack.Push(Frame);
	        Frame = frame;
	    }
	    protected bool IsStackEmpty()
	    {
	        return _stack.Count == 0;
	    }
	}
	public abstract class FilePatternContextLinear
	   : FilePatternContext<FilePatternContextLinear.FrameData>
	{
	    public FilePatternContextLinear(IFileLinearPattern pattern)
	    {
	        Pattern = pattern;
	    }
	    public override FilePatternTestResult Test(IFileSystemFile file)
	    {
	        if (IsStackEmpty())
	        {
	            throw new InvalidOperationException();// SR.CannotTestFile);
	        }
	        if (!Frame.IsNotApplicable && IsLastSegment() && TestMatchingSegment(file.Name))
	        {
	            return FilePatternTestResult.Success(CalculateStem(file));
	        }
	        return FilePatternTestResult.Failed;
	    }
	    public override void PushDirectory(IFileSystemDirectory directory)
	    {
	        // copy the current frame
	        FrameData frame = Frame;
	        if (IsStackEmpty() || Frame.IsNotApplicable)
	        {
	            // when the stack is being initialized
	            // or no change is required.
	        }
	        else if (!TestMatchingSegment(directory.Name))
	        {
	            // nothing down this path is affected by this pattern
	            frame.IsNotApplicable = true;
	        }
	        else
	        {
	            // Determine this frame's contribution to the stem (if any)
	            IFilePathSegment segment = Pattern.Segments[Frame.SegmentIndex];
	            if (frame.InStem || segment.CanProduceStem)
	            {
	                frame.InStem = true;
	                frame.StemItems.Add(directory.Name);
	            }
	            // directory matches segment, advance position in pattern
	            frame.SegmentIndex = frame.SegmentIndex + 1;
	        }
	        PushDataFrame(frame);
	    }
	    public struct FrameData
	    {
	        public bool IsNotApplicable;
	        public int SegmentIndex;
	        public bool InStem;
	        private IList<string> _stemItems;
	        public IList<string> StemItems
	        {
	            get { return _stemItems ?? (_stemItems = new List<string>()); }
	        }
	        public string Stem
	        {
	            get { return _stemItems == null ? null : string.Join("/", _stemItems); }
	        }
	    }
	    protected IFileLinearPattern Pattern { get; }
	    protected bool IsLastSegment()
	    {
	        return Frame.SegmentIndex == Pattern.Segments.Count - 1;
	    }
	    protected bool TestMatchingSegment(string value)
	    {
	        if (Frame.SegmentIndex >= Pattern.Segments.Count)
	        {
	            return false;
	        }
	        return Pattern.Segments[Frame.SegmentIndex].Match(value);
	    }
	    protected string CalculateStem(IFileSystemInfo matchedFile)
	    {
	        return FileMatcherContext.CombinePath(Frame.Stem, matchedFile.Name);
	    }
	}
	public class FilePatternContextLinearExclude : FilePatternContextLinear
	{
	    public FilePatternContextLinearExclude(IFileLinearPattern pattern)
	        : base(pattern)
	    {
	    }
	    public override bool Test(IFileSystemDirectory directory)
	    {
	        if (IsStackEmpty())
	        {
	            throw new InvalidOperationException();// SR.CannotTestDirectory);
	        }
	        if (Frame.IsNotApplicable)
	        {
	            return false;
	        }
	        return IsLastSegment() && TestMatchingSegment(directory.Name);
	    }
	}
	public class FilePatternContextLinearInclude : FilePatternContextLinear
	{
	    public FilePatternContextLinearInclude(IFileLinearPattern pattern)
	        : base(pattern)
	    {
	    }
	    public override void Declare(Action<IFilePathSegment, bool> onDeclare)
	    {
	        if (IsStackEmpty())
	        {
	            throw new InvalidOperationException();// SR.CannotDeclarePathSegment);
	        }
	        if (Frame.IsNotApplicable)
	        {
	            return;
	        }
	        if (Frame.SegmentIndex < Pattern.Segments.Count)
	        {
	            onDeclare(Pattern.Segments[Frame.SegmentIndex], IsLastSegment());
	        }
	    }
	    public override bool Test(IFileSystemDirectory directory)
	    {
	        if (IsStackEmpty())
	        {
	            throw new InvalidOperationException();// SR.CannotTestDirectory);
	        }
	        if (Frame.IsNotApplicable)
	        {
	            return false;
	        }
	        return !IsLastSegment() && TestMatchingSegment(directory.Name);
	    }
	}
	public abstract class FilePatternContextRagged : FilePatternContext<FilePatternContextRagged.FrameData>
	{
	    public FilePatternContextRagged(IFileRaggedPattern pattern)
	    {
	        Pattern = pattern;
	    }
	    public override FilePatternTestResult Test(IFileSystemFile file)
	    {
	        if (IsStackEmpty())
	        {
	            throw new InvalidOperationException();// SR.CannotTestFile);
	        }
	        if (!Frame.IsNotApplicable && IsEndingGroup() && TestMatchingGroup(file))
	        {
	            return FilePatternTestResult.Success(CalculateStem(file));
	        }
	        return FilePatternTestResult.Failed;
	    }
	    public sealed override void PushDirectory(IFileSystemDirectory directory)
	    {
	        // copy the current frame
	        FrameData frame = Frame;
	        if (IsStackEmpty())
	        {
	            // initializing
	            frame.SegmentGroupIndex = -1;
	            frame.SegmentGroup = Pattern.StartsWith;
	        }
	        else if (Frame.IsNotApplicable)
	        {
	            // no change
	        }
	        else if (IsStartingGroup())
	        {
	            if (!TestMatchingSegment(directory.Name))
	            {
	                // nothing down this path is affected by this pattern
	                frame.IsNotApplicable = true;
	            }
	            else
	            {
	                // starting path incrementally satisfied
	                frame.SegmentIndex += 1;
	            }
	        }
	        else if (!IsStartingGroup() && directory.Name == "..")
	        {
	            // any parent path segment is not applicable in **
	            frame.IsNotApplicable = true;
	        }
	        else if (!IsStartingGroup() && !IsEndingGroup() && TestMatchingGroup(directory))
	        {
	            frame.SegmentIndex = Frame.SegmentGroup.Count;
	            frame.BacktrackAvailable = 0;
	        }
	        else
	        {
	            // increase directory backtrack length
	            frame.BacktrackAvailable += 1;
	        }
	        if (frame.InStem)
	        {
	            frame.StemItems.Add(directory.Name);
	        }
	        while (
	            frame.SegmentIndex == frame.SegmentGroup.Count &&
	            frame.SegmentGroupIndex != Pattern.Contains.Count)
	        {
	            frame.SegmentGroupIndex += 1;
	            frame.SegmentIndex = 0;
	            if (frame.SegmentGroupIndex < Pattern.Contains.Count)
	            {
	                frame.SegmentGroup = Pattern.Contains[frame.SegmentGroupIndex];
	            }
	            else
	            {
	                frame.SegmentGroup = Pattern.EndsWith;
	            }
	            // We now care about the stem
	            frame.InStem = true;
	        }
	        PushDataFrame(frame);
	    }
	    public override void PopDirectory()
	    {
	        base.PopDirectory();
	        if (Frame.StemItems.Count > 0)
	        {
	            Frame.StemItems.RemoveAt(Frame.StemItems.Count - 1);
	        }
	    }
	    public struct FrameData
	    {
	        public bool IsNotApplicable;
	        public int SegmentGroupIndex;
	        public IList<IFilePathSegment> SegmentGroup;
	        public int BacktrackAvailable;
	        public int SegmentIndex;
	        public bool InStem;
	        private IList<string> _stemItems;
	        public IList<string> StemItems
	        {
	            get { return _stemItems ?? (_stemItems = new List<string>()); }
	        }
	        public string Stem
	        {
	            get { return _stemItems == null ? null : string.Join("/", _stemItems); }
	        }
	    }
	    protected IFileRaggedPattern Pattern { get; }
	    protected bool IsStartingGroup()
	    {
	        return Frame.SegmentGroupIndex == -1;
	    }
	    protected bool IsEndingGroup()
	    {
	        return Frame.SegmentGroupIndex == Pattern.Contains.Count;
	    }
	    protected bool TestMatchingSegment(string value)
	    {
	        if (Frame.SegmentIndex >= Frame.SegmentGroup.Count)
	        {
	            return false;
	        }
	        return Frame.SegmentGroup[Frame.SegmentIndex].Match(value);
	    }
	    protected bool TestMatchingGroup(IFileSystemInfo value)
	    {
	        int groupLength = Frame.SegmentGroup.Count;
	        int backtrackLength = Frame.BacktrackAvailable + 1;
	        if (backtrackLength < groupLength)
	        {
	            return false;
	        }
	        var scan = value;
	        for (int index = 0; index != groupLength; ++index)
	        {
	            IFilePathSegment segment = Frame.SegmentGroup[groupLength - index - 1];
	            if (!segment.Match(scan.Name))
	            {
	                return false;
	            }
	            scan = scan.ParentDirectory;
	        }
	        return true;
	    }
	    protected string CalculateStem(IFileSystemInfo matchedFile)
	    {
	        return FileMatcherContext.CombinePath(Frame.Stem, matchedFile.Name);
	    }
	}
	public class FilePatternContextRaggedExclude : FilePatternContextRagged
	{
	    public FilePatternContextRaggedExclude(IFileRaggedPattern pattern)
	        : base(pattern)
	    {
	    }
	    public override bool Test(IFileSystemDirectory directory)
	    {
	        if (IsStackEmpty())
	        {
	            throw new InvalidOperationException();// SR.CannotTestDirectory);
	        }
	        if (Frame.IsNotApplicable)
	        {
	            return false;
	        }
	        if (IsEndingGroup() && TestMatchingGroup(directory))
	        {
	            // directory excluded with file-like pattern
	            return true;
	        }
	        if (Pattern.EndsWith.Count == 0 &&
	            Frame.SegmentGroupIndex == Pattern.Contains.Count - 1 &&
	            TestMatchingGroup(directory))
	        {
	            // directory excluded by matching up to final '/**'
	            return true;
	        }
	        return false;
	    }
	}
	public class FilePatternContextRaggedInclude : FilePatternContextRagged
	{
	    public FilePatternContextRaggedInclude(IFileRaggedPattern pattern)
	        : base(pattern)
	    {
	    }
	    public override void Declare(Action<IFilePathSegment, bool> onDeclare)
	    {
	        if (IsStackEmpty())
	        {
	            throw new InvalidOperationException();// SR.CannotDeclarePathSegment);
	        }
	        if (Frame.IsNotApplicable)
	        {
	            return;
	        }
	        if (IsStartingGroup() && Frame.SegmentIndex < Frame.SegmentGroup.Count)
	        {
	            onDeclare(Frame.SegmentGroup[Frame.SegmentIndex], false);
	        }
	        else
	        {
	            onDeclare(FilePathWildcardSegment.MatchAll, false);
	        }
	    }
	    public override bool Test(IFileSystemDirectory directory)
	    {
	        if (IsStackEmpty())
	        {
	            throw new InvalidOperationException();// SR.CannotTestDirectory);
	        }
	        if (Frame.IsNotApplicable)
	        {
	            return false;
	        }
	        if (IsStartingGroup() && !TestMatchingSegment(directory.Name))
	        {
	            // deterministic not-included
	            return false;
	        }
	        return true;
	    }
	}
	#endregion
}
#endregion
