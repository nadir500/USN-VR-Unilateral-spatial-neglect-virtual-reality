using SQLite4Unity3d;
public class GrabbedObjects  {

		//the classes in SQLITE ADDING AND RECORDING are in the same name and type as in the sqlite db tables  

		[PrimaryKey,AutoIncrement]
		public int grabb_id{get;set;}
		public int gameplay_id{get;set;}
		public bool object_grabbed{get;set;}
		public bool objects_recorded_on_pad{get;set;}
		public string objects_position{get;set;}
		public string objects_field{get;set;}
		public string obj_grabbed_by_hand{get;set;}
		






}
