using SQLite4Unity3d;
public class EnviromentSettings {

		[PrimaryKey,AutoIncrement]
		public int enviroment_id{get;set;}
		[NotNull]
		public string street_direction{get;set;}
		[NotNull]

		public int pathes_per_street{get;set;}
		[NotNull]

		public int car_speed_km{get;set;}

		public int car_span_km{get;set;}
		[NotNull]

		public int sound_mode{get;set;}



}
