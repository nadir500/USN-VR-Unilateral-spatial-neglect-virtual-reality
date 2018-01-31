using SQLite4Unity3d;
public class Gameplay {

		[PrimaryKey,AutoIncrement]
		public int gameplay_id{get;set;}
		[NotNull]
		public string street_direction{get;set;}
		[NotNull]

		public int pathes_per_direction{get;set;}
		[NotNull]

		public int car_speed_km{get;set;}

		public int car_span_km{get;set;}
		[NotNull]

		public string sound_mode{get;set;}

		public string player_name {get;set;}
		public double player_height {get;set;}
		



}
