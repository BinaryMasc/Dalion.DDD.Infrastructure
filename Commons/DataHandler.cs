using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dalion.DDD.Commons
{

    public class DataHandler : IEnumerable
    {

        private List<Row> _rows = new List<Row>();
        public int RowCount { get { return _rows.Count; } private set { } }
        public int Count { get { return RowCount; }  }

        public DataHandler(SqlDataAdapter reader)
        {
            throw new NotImplementedException();
        }


        public void Add(Row row)
        {
            _rows.Add(row);
        }
        public Row this[int element]
        {
            get
            {
                if (_rows.Count > element) return _rows[element];

                else throw new ArgumentException("Element out of range of DataHandler");
            }

            set
            {
                _rows[element] = value;
            }
        }

        //  List
        public List<object> FindObjectsByName(string pColumnName)
        {
            return _rows.Select(r => r.GetDictionary()[pColumnName]).ToList();
        }

        //  Scalar
        public object FindObjectByName(string pColumnName)
        {
            return _rows
                .Select(r => r.GetDictionary()[pColumnName])
                .First() ?? throw new Exception($"La columna '{pColumnName}' no devolvió datos.");
        }

        public string FindStringByName(string pColumnName)
        {
            return FindObjectByName(pColumnName).ToString();
        }




        IEnumerator IEnumerable.GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        public class Row : IEnumerable
        {
            public int Id;


            private Dictionary<string, object> _data;

            public Dictionary<string, object> GetDictionary() { return _data; }


            public Row() { _data = new Dictionary<string, object>(); }
            
            public void AddColumn(string columnName, object data)
            {
                _data.Add(columnName, data);
            }

            public IEnumerator GetEnumerator()
            {
                return _data.GetEnumerator();
            }
        }

        
    }
}
