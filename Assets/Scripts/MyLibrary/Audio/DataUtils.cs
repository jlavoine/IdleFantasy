using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

////////////////////////////////
/// DataUtils
/// Functions used to load data
/// from various json files.
////////////////////////////////
 
public static class DataUtils {

    //////////////////////////////////////////
    /// LoadFile()
    /// Returns whatever text is in the
    /// incoming file.
    //////////////////////////////////////////
    public static string LoadFileWithPath( string i_file ) {
        string data = "";

        if ( File.Exists( i_file ) ) {
            FileStream file = new FileStream( i_file, FileMode.Open, FileAccess.Read );
            StreamReader sr = new StreamReader( file );

            data = sr.ReadToEnd();
            sr.Close();
            file.Close();
        }

        return data;
    }
    public static string LoadFile( string i_strFile ) {
        string strPath = GetPath( i_strFile );
        return LoadFileWithPath( strPath );
	}

    //////////////////////////////////////////
    /// SaveFile()
    /// Saves the incoming data to the incoming
    /// file.
    //////////////////////////////////////////
    public static void SaveFile( string i_strFile, object i_data ) {
        string strJSON = SerializationUtils.Serialize( i_data );
        string strPath = GetPath( i_strFile );
        System.IO.File.WriteAllText( strPath, strJSON );
    }

    //////////////////////////////////////////
    /// GetPath()
    /// Returns the path (from streaming
    /// assets) of the incoming file name.
    //////////////////////////////////////////
    private static string GetPath( string i_strFile ) {
        string strPath = Application.streamingAssetsPath + "/" + i_strFile;
        return strPath;
    }

    //////////////////////////////////////////
    /// LoadFiles()
    /// Returns a list of strings from the
    /// streaming assets folder that are the
    /// files in the folder.
    //////////////////////////////////////////
    public static List<string> LoadFiles( string i_strFolder ) {
        // list of strings (representing the contents of each file in the folder)
        List<string> listContents = new List<string>();

        // get all the files in the incoming directory
        string strPath = Application.streamingAssetsPath + "/" + i_strFolder + "/";
        DirectoryInfo infoDirectory = new DirectoryInfo( strPath );
        FileInfo[] infoFiles = infoDirectory.GetFiles();

        // loop through and add the contents of the file to our list
        foreach ( FileInfo file in infoFiles ) {
            string strFilename = file.Name;

            // we only want non meta files!
            if ( strFilename.Contains( ".meta" ) )
                continue;

            // get the file's contents and add it to our list
            string strFile = LoadFile( i_strFolder + "/" + strFilename );
            listContents.Add( strFile );
        }

        return listContents;
    }

    //////////////////////////////////////////
    /// LoadData()
    /// Loads a generic dictionary of data
    /// from the streaming assets folder.
    //////////////////////////////////////////
    public static void LoadDataAsDictionary<T>( Dictionary<string, T> i_dictData, string i_strFolder ) where T : GenericData {
        // get all files in the folder (their contents)
        List<string> listFiles = LoadFiles( i_strFolder );

        // loop through each files contents..
        foreach ( string strFile in listFiles ) {
            // and get the list of data from that file's contents
            List<T> listData = JsonConvert.DeserializeObject<List<T>>( strFile );

            // loop through each piece of data and add it to our dictionary
            i_dictData = new Dictionary<string, T>();

            foreach ( T data in listData ) {
                string strID = data.ID;
                i_dictData.Add( strID, data );
            }
        }
    }

    public static void LoadData<T>( Dictionary<string, T> i_dictData, string i_strFolder ) where T : GenericData {
        // get all files in the folder (their contents)
        List<string> listFiles = LoadFiles( i_strFolder );

        // loop through each files contents..
        foreach ( string strFile in listFiles ) {
            T data = JsonConvert.DeserializeObject<T>( strFile );

            if ( i_dictData == null ) {
                i_dictData = new Dictionary<string, T>();
            }

            string strID = data.ID;
            i_dictData.Add( strID, data );
        }
    }

    public static void LoadDataAsHash<T>( Hashtable i_hash, string i_folder ) where T : GenericData {
        // get all files in the folder (their contents)
        List<string> listFiles = LoadFiles( i_folder );

        // loop through each files contents..
        foreach ( string strFile in listFiles ) {
            T data = JsonConvert.DeserializeObject<T>( strFile );

            if ( i_hash == null ) {
                i_hash = new Hashtable();
            }

            string strID = data.ID;
            i_hash.Add( strID, data );
        }
    }
}
