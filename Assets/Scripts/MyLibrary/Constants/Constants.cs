using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace MyLibrary {
    public class Constants {
        public const string TITLE_DATA_KEY = "Constants";

        // types of constants currently supported
        private const string STRING_KEY = "String";
        private const string INT_KEY = "Int";
        private const string FLOAT_KEY = "Float";
        private const string DOUBLE_KEY = "Double";
        private const string VECTOR3_KEY = "Vector3";
        private const string COLOR_KEY = "Color";
        private const string BOOL_KEY = "Bool";
        private const string STRINGLIST_KEY = "StringList";
        private const string INTLIST_KEY = "IntList";

        private static Hashtable mConstants = new Hashtable();

        public static void Init( IBasicBackend i_backend ) {
            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Initing constants", "" );

            i_backend.GetTitleData( TITLE_DATA_KEY, CreateConstantsFromJSON );
        }

        private static void CreateConstantsFromJSON( string i_JSON ) {
            Dictionary<string, ConstantsEntry> constants = JsonConvert.DeserializeObject<Dictionary<string, ConstantsEntry>>( i_JSON );

            CreateConstantsFromDictionary( constants );
        }
        
        private static void CreateConstantsFromDictionary( Dictionary<string, ConstantsEntry> i_dictionary ) {
            foreach( KeyValuePair<string, ConstantsEntry> kvp in i_dictionary ) {
                ConstantsEntry entry = kvp.Value;
                CreateConstantFromEntry( entry );
            }
        }

        private static void CreateConstantFromEntry( ConstantsEntry i_entry ) {
            switch ( i_entry.Type ) {
                case STRING_KEY:
                    mConstants[i_entry.ID] = i_entry.Value;
                    break;
                case INT_KEY:
                    mConstants[i_entry.ID] = int.Parse( i_entry.Value );
                    break;
                case BOOL_KEY:
                    mConstants[i_entry.ID] = bool.Parse( i_entry.Value );
                    break;
                case FLOAT_KEY:
                    mConstants[i_entry.ID] = float.Parse( i_entry.Value );
                    break;
                case DOUBLE_KEY:
                    mConstants[i_entry.ID] = double.Parse( i_entry.Value );
                    break;
                case COLOR_KEY:
                    mConstants[i_entry.ID] = ParseColor( i_entry.Value );
                    break;
                case VECTOR3_KEY:
                    mConstants[i_entry.ID] = ParseVector3( i_entry.Value );
                    break;
                case STRINGLIST_KEY:
                    mConstants[i_entry.ID] = ParseStringList( i_entry.Value );
                    break;
                case INTLIST_KEY:
                    mConstants[i_entry.ID] = ParseList<int>( i_entry.Value );
                    break;
                default:
                    MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "Illegal constant type " + i_entry.Type + " for key " + i_entry.ID, "" );
                    break;
            }
        }

        public static T GetConstant<T>( string strKey, T defaultVal ) {
            if ( mConstants.Count == 0 ) {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Fatal, "Constants being accessed before init!", "" );
            }

            T data = defaultVal;
            if ( mConstants.ContainsKey( strKey ) ) {
                data = (T) mConstants[strKey];
            }
            else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Fatal, "No such constant for key " + strKey, "" );
            }

            return data;
        }
        public static T GetConstant<T>( string strKey ) {
            return GetConstant<T>( strKey, default( T ) );
        }

        public static Vector3 ParseVector3( string i_str ) {
            Vector3 vector = new Vector3( 0, 0, 0 );
            String[] arrayVector3 = i_str.Split( ","[0] );
            if ( arrayVector3.Length == 3 ) {
                vector = new Vector3(
                                     float.Parse( arrayVector3[0] ),
                                     float.Parse( arrayVector3[1] ),
                                     float.Parse( arrayVector3[2] ) );
            }
            else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Fatal, "Illegal vector3 parsing for " + i_str + " -- reverting to 0,0,0", "" );
            }

            return vector;
        }
        	
        public static List<string> ParseStringList( string i_str ) {
            List<string> list = new List<string>();
            String[] array = i_str.Split( ","[0] );

            for ( int i = 0; i < array.Length; ++i )
                list.Add( array[i] );

            return list;
        }
        public static List<T> ParseList<T>( string i_str ) {
            List<T> list = new List<T>();
            String[] array = i_str.Split( ","[0] );
            TypeConverter converter = TypeDescriptor.GetConverter( typeof( T ) );

            for ( int i = 0; i < array.Length; ++i ) {
                T val = (T) converter.ConvertFromString( array[i] );
                list.Add( val );
            }

            return list;
        }

        private static Color ParseColor( string i_str ) {
            Color color = Color.white;
            String[] arrayColor = i_str.Split( ","[0] );
            if ( arrayColor.Length == 4 ) {
                color = new Color(
                                  float.Parse( arrayColor[0] ) / 255,       // why unity's color picker is 0-255 but the code wants floats, I don't know...
                                  float.Parse( arrayColor[1] ) / 255,
                                  float.Parse( arrayColor[2] ) / 255,
                                  float.Parse( arrayColor[3] ) / 255 );
            }
            else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Fatal, "Illegal color parsing for " + i_str + " -- reverting to white", "" );
            }

            return color;
        }
    }
}
