using SQLite4Unity3d;
public class GatheringObjects  {

		[PrimaryKey,AutoIncrement]
		public int gather_id{get;set;}
		[NotNull]
		public int player_id{get;set;}
		[NotNull]

		public int object_grabbed{get;set;}
		[NotNull]

		public int objects_gathered{get;set;}
		[NotNull]

		public int objects_recorded_on_pad{get;set;}




}
