using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService
{
    private SQLiteConnection _connection;
    public DataService(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }
    //create gameplay row in the db 
    public int CreateGameplay()
    {
        Gameplay gamePlay = new Gameplay
        {
            street_direction = ExperimentParameters.streetsDirections,
            lanes_per_direction = ExperimentParameters.lanes_per_direction,
            car_speed_km = ExperimentParameters.carsSpeed,
            car_span_second = ExperimentParameters.distanceBetweenCars,
            sound_mode = ExperimentParameters.soundDirections,
            player_name = ExperimentParameters.player_name,
            player_height = (double)ExperimentParameters.lengthOfPatient
        };
     
      return _connection.Insert(gamePlay);
    }
    //create a crossingroaddata row in the db
    public void CreateRoadCrossingData(ObservedData observedData)
    {
        StreetCrossingData streetCrossingData;

        for (int i = 0; i < 2; i++)
        {
            streetCrossingData = new StreetCrossingData
            {
                gameplay_id = ExperimentParameters.gameplay_id,  //storing from the static variable in the class
                traffic_flow_towards = observedData.traffic_towards_flow[i],
                current_time_span = double.Parse(observedData.current_time_span[i].ToString("F3")),
                current_distance_nearest_car = 0, //for now 
                gazing_car = observedData.isLookingAtCar[i],
                gazing_nearest_car = false, //for now 
                after_collision_frame = observedData.is_hit_by_car[i],
                person_x = (double)observedData.playerPositions[i].x,
                person_y = (double)observedData.playerPositions[i].y,
                person_z = (double)observedData.playerPositions[i].z,
                head_rotation_y = (double)observedData.playerHeadRotations[i]
            };
           
            _connection.Insert(streetCrossingData);
        }
    }
    //getting the current gameplay id 
    public int GetGameplayIDFromDatabase()  //stored in playerPref 
    {
        int rows = _connection.Table<Gameplay>().Select(x => x.gameplay_id).Count();
        if(rows == 0 )
        {
            return 0;
        }
        else
            return rows;
    }
    public IEnumerable<Gameplay> GetGameplayTable()
    {
        return _connection.Table<Gameplay>();
    }
    public IEnumerable<StreetCrossingData> GetRoadCrossingDataTable()
    {
        return _connection.Table<StreetCrossingData>();
    }
    //recording objects collecting data from hands 
    public void CreateCollectedObjectsRow(Collected_Objects collected_Objects)
    {
        Collected_Objects collected_ = new Collected_Objects {
                gameplay_id = ExperimentParameters.gameplay_id,
                obj_number = collected_Objects.obj_number,
                obj_position = collected_Objects.obj_position,
                obj_field = collected_Objects.obj_field,
                obj_recorded_on_pad = collected_Objects.obj_recorded_on_pad,
                obj_recorded_after_attempt = collected_Objects.obj_recorded_after_attempt,
                obj_collected = collected_Objects.obj_collected,
                obj_collected_by_hand = collected_Objects.obj_collected_by_hand
        };
      
        _connection.Insert(collected_);
    }
    
    public void UpdateCollectedObjectByClicking(int id, bool obj_collected,char obj_collected_by_hand)
    {
        Collected_Objects temp_collected_Objects = _connection.Table<Collected_Objects>().Where(x => x.obj_number == id).First();
        temp_collected_Objects.obj_collected = obj_collected;
        temp_collected_Objects.obj_collected_by_hand = obj_collected_by_hand.ToString();
      
        _connection.Update(temp_collected_Objects);
    }


    /* JUNK CODE */
    public void UpdateCollectedObjectOnPad(int id , bool recorded_on_pad)
    {
        Collected_Objects temp_collected_Objects = _connection.Table<Collected_Objects>().Where(x => x.obj_number == id).First();
        temp_collected_Objects.obj_recorded_on_pad = recorded_on_pad;
      
        _connection.Insert(temp_collected_Objects);
    }

}
