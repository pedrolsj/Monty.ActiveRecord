#Monty.ActiveRecord

## Synopsis

Easy to use database layer for c#
It's like the Castle.ActiveRecord but without the NHibernate layer to couse trouble. At this stage you can save/delete/search on MySQL and SQL Server databases.

## Features

*SQL-less, xml-less table to class mapping
*Joined classes
*Relations 1-1 and 1-n
*Cascade effect for save/delete
*MySQL and SQL server suport

## Code Example

```csharp
//Class Definition and Mapping

[ActiveRecord("montyTJob")]
public class Job : ActiveRecordBase<Job>
{
	[PrimaryKey("JobId", Generator = PrimaryKeyType.Identity)]
	public int Id { get; set; }

	[Property("Name", ColumnType = "varchar(200)", NotNull = true, Length = 200)]
	public string Name { get; set; }
}		

[ActiveRecord("montyTPerson")]
public class Person : ActiveRecordBase<Person>
{
	[PrimaryKey("PersonId", Generator = PrimaryKeyType.Identity)]
	public int Id { get; set; }

	[Property("Name", ColumnType = "varchar(200)", NotNull = true, Length = 200)]
	public string Name { get; set; }

	[Property("Birthday", NotNull = false)]
	public DateTime? Birthday { get; set; }

	private Job _CurrentJob;
	[BelongsTo("CurrentJob", NotNull = false)]
	public Job CurrentJob
	{
		get
		{
			if (_CurrentJob == null)
				_CurrentJob = LazyLoad<Job>("CurrentJob");

			return _CurrentJob;
		}
		set { _CurrentJob = value; }
	}
}

//Insert/Update
Person person = new Person();

person.Name = name;
person.Birthday = birthday;
person.CurrentJob = currentJob;

Person.Save();

//Delete
Person.Delete();

//Find item by primary key
Person.Find(1);

//Find all items
Person.FindAll(1);

//Find all items by criteria
Criteria criteria = Criteria.For<Person>();

criteria
	.Add(criteria.Like("Name", letter, mode))
	.Add(criteria.Asc("Name"));

List<Person> people = Person.FindAll(criteria);

//Paginate
Criteria criteria = Criteria.For<Person>();

criteria
	.Add(criteria.Like("Name", letter, mode))
	.Add(criteria.Asc("Name"));

int numberOfPage = 0;
int numberOfItems = 0;

List<Person> people = Person.SlicedFindAll(criteria, currentPage, pageSize, out numberOfPage, out numberOfItems);

//Joined bases

[ActiveRecord("artifactTArtifact"), JoinedBase]
public class Artifact : ActiveRecordBase
{
	[PrimaryKey("ArtifactId", Generator = PrimaryKeyType.Identity)]
	public int Id { get; set; }

	[Property("Title", Length = 100, NotNull = true, ColumnType = "varchar(100)")]
	public string Title { get; set; }
}

[ActiveRecord("artifactTImage"), JoinedBase]
public class Image : Artifact
{
	[PrimaryKey(PrimaryKeyType.Assigned, "ArtifactId")]
	public int ImageId { get; set; }

	[Property("FileName", Length = 100, NotNull = true, ColumnType = "varchar(100)")]
	public string FileName { get; set; }
}
```

## Contributors

Me @pedrolsj