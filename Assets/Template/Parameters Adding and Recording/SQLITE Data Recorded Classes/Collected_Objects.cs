using SQLite4Unity3d;
public class Collected_Objects  {

		//the classes in SQLITE ADDING AND RECORDING are in the same name and type as in the sqlite db tables  

		[PrimaryKey,AutoIncrement]
		public int collected_objects_id{get;set;}
		public int gameplay_id{get;set;}
		public int  obj_number{get;set;}
		public string obj_position{get;set;}
		public string obj_field{get;set;}
		public bool obj_recorded_on_pad{get;set;}
		public bool obj_collected{get;set;}
		public string obj_collected_by_hand{get;set;}
		






}
