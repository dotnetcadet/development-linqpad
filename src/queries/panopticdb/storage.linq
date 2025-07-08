<Query Kind="Program" />

using Assimalign.PanopticDb.Storage;

void Main()
{
	var storage = default(IStorage);
	
	if (storage.IsBlobStorage())
	{
		
	}
}


public interface IStorage {
	StorageType StorageType { get; }
	IStorageIterator GetIterator();
}
public interface IStorageIterator : IEnumerable<IStorageSegment>, IAsyncEnumerable<IStorageSegment> { // Iterator 
}
public interface IStorageSegment {
	StorageSegmentType SegmentType { get; }
}
public interface IStorageSegmentComposite : IStorageSegment {
	IStorageIterator GetIterator();
}
public interface IStoragePage {
	StoragePageType PageType { get; }
}
public interface IStoragePageTuple {
	
}

public enum StorageType {
	Unknown = -1,
	Sql,
	Document,
	Blob,
	Graph
}
public enum StorageSegmentType {
	
}
public enum StoragePageType {
	
}


#region Extensions
namespace Assimalign.PanopticDb.Storage {
	public static class StorageExtensions {
		public static bool IsSqlStorage(this IStorage storage) {
			if (storage is null) {
				throw new ArgumentNullException(nameof(storage));
			}
			return storage.StorageType == StorageType.Sql;
		}
		public static bool IsDocumentStorage(this IStorage storage) {
			if (storage is null) {
				throw new ArgumentNullException(nameof(storage));
			}
			return storage.StorageType == StorageType.Document;
		}
		public static bool IsBlobStorage(this IStorage storage) {
			if (storage is null) {
				throw new ArgumentNullException(nameof(storage));
			}
			return storage.StorageType == StorageType.Blob;
		}
		public static bool IsGraphStorage(this IStorage storage) {
			if (storage is null) {
				throw new ArgumentNullException(nameof(storage));
			}
			return storage.StorageType == StorageType.Blob;
		}
	}
}
#endregion