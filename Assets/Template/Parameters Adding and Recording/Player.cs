﻿using SQLite4Unity3d;
public class Player  {
        [PrimaryKey,AutoIncrement]
        public int player_id {get;set;}
        [NotNull]
        public string player_name {get;set;}
        [NotNull]
        public double player_height {get;set;}
	
}