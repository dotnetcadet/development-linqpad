<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Runtime.Serialization</Namespace>
</Query>

void Main()
{


}

/// <summary> 
///
/// </summary>
/// <remarks>
/// Crontab expression format: <para />
/// <para /> 
/// *****	<para />
/// -----	 <para />
/// | | | | |	<para />
/// | | | | +-------- day of week (0 - 6) (Sunday=0)<para />
/// | | | +---------- month (1 - 12)<para />
/// | | +------------ day of month (1 - 31) <para />
/// | +-------------- hour (0 - 23) <para />
/// +---------------- min (0 - 59) <para />
/// 
/// Star (*) in the value field above means all legal values as in
/// braces for that column. The value column can have a * or a list
/// of elements separated by commas. An element is either a number in
/// the ranges shown above or two numbers in the range separated by a
/// hyphen (meaning an inclusive range).
///
/// Source: http://www.adminschoice.com/docs/crontab.htm
///
///
/// Six-part expression format:
///
/// * * * * * *
/// - - - - - -
/// ||||||
/// | | | | | +--- day of week (0 - 6) (Sunday=0)
/// | | | | +----- month (1 - 12)
/// | | | +------- day of month (1 - 31)
/// | | +--------- hour (0 - 23)
/// | +----------- min (0 - 59)
/// +------------- sec (0 - 59)
/// 
/// The six-part expression behaves similarly to the traditional
/// crontab format except that it can denotate more precise schedules
/// that use a seconds component.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Crontab : IEquatable<Crontab>, IEnumerable<DateTime>, ISerializable, IFormattable
{
	public const char RangValue = '-';
	public const char StepValue = '/';
	public const char Any = '*';
	public const char ListSeparator = ',';


	/** <summary> */
	/** This is a test */
	/** another test */
	/** </summary> */
	private Crontab(CrontabField minute, CrontabField hour, CrontabField dayOfMonth, CrontabField month, CrontabField dayOfWeek)
	{
		this.Minute = minute;
		this.Hour = hour;
		this.DayOfMonth = dayOfMonth;
		this.Month = month;
		this.DayOfWeek = dayOfWeek;
	}

	/// <summary>
	/// 
	/// </summary>
	public CrontabField Minute { get; }

	/// <summary>
	/// 
	/// </summary>
	public CrontabField Hour { get; }

	/// <summary>
	/// 
	/// </summary>
	public CrontabField DayOfMonth { get; }

	/// <summary>
	/// 
	/// </summary>
	public CrontabField Month { get; }

	/// <summary>
	/// 
	/// </summary>
	public CrontabField DayOfWeek { get; }

	/// <summary>
	/// Get's the amount of time until the next occurrence from the current local time.
	/// </summary>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public TimeSpan GetTimeSpan()
	{
		return GetDateTime().Subtract(DateTime.Now);
	}

	/// <summary>
	/// Get's the next occurrence in DateTime from the current local time.
	/// </summary>
	/// <returns></returns>
	public DateTime GetDateTime()
	{
		return GetDateTime(DateTime.Now);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="start"></param>
	/// <returns></returns>
	public DateTime GetDateTime(DateTime start)
	{
		var index = 0;
		var next = new DateTime(start.Year, 1, 1, 0, 0, 0);

	restart:
		for (int a = 0; a < 12; a++)
		{
			if (!Month.Occurrences.Contains(a + 1))
			{
				next = next.AddMonths(1);
				continue;
			}
			for (int b = 0; b < DateTime.DaysInMonth(next.Year, a + 1); b++)
			{
				if (!DayOfMonth.Occurrences.Contains(b + 1) || !DayOfWeek.Occurrences.Contains((int)next.DayOfWeek))
				{
					next = next.AddDays(1);
					continue;
				}
				for (int c = 0; c < 24; c++)
				{
					if (!Hour.Occurrences.Contains(c))
					{
						next = next.AddHours(1);
						continue;
					}
					for (int d = 0; d < 60; d++)
					{
						if (Minute.Occurrences.Contains(d) && next > start)
						{
							return next;
						}
						else
						{
							next = next.AddMinutes(1);
						}
					}
				}
			}
		}

		if (index >= 2)
		{
			throw new Exception("The provided expression was parsed but has an invalid tab. ");
		}
		index++;
		goto restart;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public bool Equals(Crontab other)
	{
		return
			this.Minute.Expression == other.Minute.Expression &&
			this.Hour.Expression == other.Hour.Expression &&
			this.DayOfMonth.Expression == other.DayOfMonth.Expression &&
			this.Month.Expression == other.Month.Expression &&
			this.DayOfWeek.Expression == other.DayOfWeek.Expression;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="format"></param>
	/// <param name="formatProvider"></param>
	/// <returns></returns>
	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		return format switch
		{
			"E" => $"{Minute} {Hour} {DayOfMonth} {Month} {DayOfWeek}",
			_ => $"{Minute} {Hour} {DayOfMonth} {Month} {DayOfWeek}"
		};
	}


	#region Overloads

	/// <inheritdoc/>
	public override string ToString()
	{
		return ToString("E", default);
	}

	/// <inheritdoc/>
	public override bool Equals(object? instance)
	{
		if (instance is Crontab crontab)
		{
			return Equals(crontab);
		}
		return false;
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		// TODO: Temporary generation
		return HashCode.Combine(typeof(Crontab), ToString());
	}


	#endregion

	#region Operators

	public static bool operator ==(Crontab left, Crontab right) => left.Equals(right);
	public static bool operator !=(Crontab left, Crontab right) => !left.Equals(right);
	public static implicit operator Crontab(string expression) => Crontab.Parse(expression);

	#endregion

	#region Helpers

	public static Crontab Parse(string expression)
	{
		var segments = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		// Let's ensure that the expression segments has the proper length 
		if (segments.Length != 5)
		{
			throw new ArgumentException("The expression is not in the proper format.");
		}

		var minute = default(CrontabField);      // index - 1 or 2
		var hour = default(CrontabField);
		var dayOfMonth = default(CrontabField);
		var month = default(CrontabField);
		var dayOfWeek = default(CrontabField);

		for (int i = 0; i < segments.Length; i++)
		{
			if (i == 0)
			{
				minute = CrontabField.ParseMinute(segments[i]);
				continue;
			}
			if (i == 1)
			{
				hour = CrontabField.ParseHour(segments[i]);
				continue;
			}
			if (i == 2)
			{
				dayOfMonth = CrontabField.ParseDayOfMonth(segments[i]);
				continue;
			}
			if (i == 3)
			{
				month = CrontabField.ParseMonth(segments[i]);
				continue;
			}
			if (i == 4)
			{
				dayOfWeek = CrontabField.ParseDayOfWeek(segments[i]);
				continue;
			}
		}

		return new Crontab(minute, hour, dayOfMonth, month, dayOfWeek);
	}

	#endregion
	
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info is null)
		{
			throw new ArgumentNullException("info");
		}

		info.AddValue("crontabExpression", this.ToString());
	}

	#region Crontab Enumeration
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	public IEnumerator<DateTime> GetEnumerator()
	{
		return new CrontabEnumerator(this);
	}



	private class CrontabEnumerator : IEnumerator<DateTime>
	{
		private readonly Crontab crontab;
		private DateTime? index;

		public CrontabEnumerator(Crontab crontab)
		{
			this.crontab = crontab;
		}

		public DateTime Current
		{
			get
			{
				if (!index.HasValue)
				{
					index = crontab.GetDateTime();
					return index.GetValueOrDefault();
				}
				else
				{
					index = crontab.GetDateTime(index.Value);
					return index.GetValueOrDefault();
				}
			}
		}

		object IEnumerator.Current => this.Current;

		public void Dispose()
		{

		}

		public bool MoveNext() => true;

		public void Reset()
		{
			index = null;
		}
	}
	#endregion
}




public readonly partial struct Crontab 
{
	
	
	
	public partial struct Field 
	{
		
	}
}
/// <summary>
/// 
/// </summary>
public readonly struct CrontabField
{
	private CrontabField(CrontabFieldKind kind, string expression, int minBoundary, int maxBoundary, int[] occurances)
	{
		this.Kind = kind;
		this.Expression = expression;
		this.MinBoundary = minBoundary;
		this.MaxBoundary = maxBoundary;
		this.Occurrences = occurances;
	}

	/// <summary>
	/// 
	/// </summary>
	public bool IsAll => this.Expression == "*";
	/// <summary>
	/// An array of all the possible occurrences 
	/// </summary>
	public int[] Occurrences { get; }
	/// <summary>
	/// 
	/// </summary>
	public CrontabFieldKind Kind { get; }
	/// <summary>
	/// 
	/// </summary>
	public string Expression { get; }
	/// <summary>
	/// 
	/// </summary>
	public int MaxBoundary { get; }
	/// <summary>
	/// 
	/// </summary>
	public int MinBoundary { get; }

	public static CrontabField ParseMinute(string expression)
	{
		return new CrontabField(
			CrontabFieldKind.Minute,
			expression,
			0,
			59,
			GetOccurances(expression, 0, 59));
	}
	public static CrontabField ParseHour(string expression)
	{
		return new CrontabField(
			CrontabFieldKind.Minute,
			expression,
			0,
			23,
			GetOccurances(expression, 0, 23));
	}
	public static CrontabField ParseDayOfMonth(string expression)
	{
		return new CrontabField(
			CrontabFieldKind.Minute,
			expression,
			1,
			31,
			GetOccurances(expression, 1, 31)); ;
	}
	public static CrontabField ParseMonth(string expression)
	{
		return new CrontabField(
			CrontabFieldKind.Minute,
			expression,
			1,
			12,
			GetOccurances(expression, 1, 12));
	}
	public static CrontabField ParseDayOfWeek(string expression)
	{
		return new CrontabField(
			CrontabFieldKind.Minute,
			expression,
			0,
			6,
			GetOccurances(expression, 0, 6));
	}
	private static int[] GetOccurances(string expression, int min, int max)
	{
		if (expression == "*")
		{
			var seed = min;
			var occurrences = new int[max - min + 1];
			for (int i = 0; i < occurrences.Length; i++)
			{
				occurrences[i] = seed;
				seed++;
			}
			return occurrences;
		}
		if (expression.Contains('/'))
		{
			var steps = expression.Split('/');
			var boundariesStep = steps[0];
			var intervalsStep = steps[1];

			// Check for invalid step format
			if (steps.Length != 2)
			{
				throw new FormatException($"The following expression '{expression}' has either more than one step delimiter -> '/', or is invalid.");
			}
			if (boundariesStep.Equals("*"))
			{
				var occurrences = new List<int>();
				// Indicates a list of varied intervals between 0 and 59
				// Example: */2,5,7
				//      Occurrence A: 2, 4, 6, 8,...
				//      Occurrence B: 5, 10, 15,....
				//      Occurrence C: 7, 14, 21, 28,...
				// NOTE: Once the occurrence list has been built, select only distinct int.
				//       The varied interval can sometimes have duplicate values
				if (intervalsStep.Contains(','))
				{
					var intervals = intervalsStep.Split(',');

					for (int i = 0; i < intervals.Length; i++)
					{
						occurrences.AddRange(GetOccurances($"*/{intervals[i]}", min, max));
					}
				}
				else
				{
					var interval = int.Parse(intervalsStep);
					if (interval > max)
					{
						throw new ArgumentException($"The step value '{interval}' in expression '{expression}' cannot be greater than '{max}'.");
					}
					for (int i = min; i <= max; i = i + interval)
					{
						occurrences.Add(i);
					}
				}

				occurrences.Sort();
				return occurrences.Distinct().ToArray();
			}
			// Check for a list of boundaries
			if (boundariesStep.Contains(','))
			{
				var occurrences = new List<int>();
				var boundariesList = boundariesStep.Split(',');

				for (int i = 0; i < boundariesList.Length; i++)
				{
					var lower = min;
					var upper = max;

					// Is the current boundary a range or single value
					if (boundariesList[i].Contains('-'))
					{
						lower = int.Parse(boundariesList[i].Split('-')[0]);
						upper = int.Parse(boundariesList[i].Split('-')[1]);
					}
					else
					{
						lower = int.Parse(boundariesList[i]);
					}
					// Check if the parse boundaries are greater of less than the default boundaries
					if (lower < min || upper > max)
					{
						throw new ArgumentOutOfRangeException("");
					}
					// Now check if the intervals step is also a list
					if (intervalsStep.Contains(','))
					{
						var intervals = intervalsStep.Split(',');

						for (int c = 0; c < intervals.Length; c++)
						{
							occurrences.AddRange(GetOccurances($"*/{intervals[c]}", lower, upper));
						}
					}
					else
					{
						occurrences.AddRange(GetOccurances($"*/{intervalsStep}", lower, upper));
					}
				}

				occurrences.Sort();
				return occurrences.Distinct().ToArray();
			}
			// Check if boundaries is a range
			if (boundariesStep.Contains('-'))
			{
				var lower = int.Parse(boundariesStep.Split('-')[0]);
				var upper = int.Parse(boundariesStep.Split('-')[1]);
				var occurrences = new List<int>();

				// Check if the parse boundaries are greater of less than the default boundaries
				if (lower < min || upper > max)
				{
					throw new ArgumentOutOfRangeException("");
				}
				if (intervalsStep.Contains(','))
				{
					var intervals = intervalsStep.Split(',');
					for (int i = 0; i < intervals.Length; i++)
					{
						occurrences.AddRange(GetOccurances($"{lower}-{upper}/{intervals[i]}", min, max));
					}
				}
				else
				{
					var interval = int.Parse(intervalsStep);

					for (int i = lower; i < upper; i = i + interval)
					{
						occurrences.Add(i);
					}
				}

				occurrences.Sort();
				return occurrences.Distinct().ToArray();
			}
		}
		if (expression.Contains(','))
		{
			var boundaies = expression.Split(',');
			var occurrences = new List<int>();

			for (int i = 0; i < boundaies.Length; i++)
			{
				if (boundaies[i].Contains('-'))
				{
					var lower = int.Parse(boundaies[i].Split('-')[0]);
					var upper = int.Parse(boundaies[i].Split('-')[1]);

					occurrences.AddRange(GetOccurances($"{lower}-{upper}", min, max));
				}
				else
				{
					occurrences.AddRange(GetOccurances(boundaies[i], min, max));
				}
			}

			occurrences.Sort();
			return occurrences.ToArray();
		}
		if (expression.Contains('-'))
		{
			var lower = int.Parse(expression.Split('-')[0]);
			var upper = int.Parse(expression.Split('-')[1]);
			var occurrences = new List<int>();

			if (lower >= upper)
			{
				throw new ArgumentException($"The provided range {lower}-{upper} within the given expression is invalid.");
			}
			for (int i = lower; i < upper + 1; i++)
			{
				occurrences.Add(i);
			}

			occurrences.Sort();
			return occurrences.ToArray();
		}
		else
		{
			var value = int.Parse(expression);
			if (value > max || value < min)
			{
				throw new ArgumentOutOfRangeException("expression", value, $"The value(s) must be between {min} and {max}. The value {value} within the provided expression failed.");
			}
			return new int[1] { value };
		}
	}


	public override string ToString()
	{
		return this.Expression;
	}
}


public enum CrontabFieldKind
{
	Second = 0, // Keep in order of appearance in expression
	Minute = 1,
	Hour = 2,
	DayOfMonth = 3,
	Month = 4,
	DayOfWeek = 5
}


public sealed class CrontabBuilder
{



	public Crontab Build()
	{
		return default;
	}
}