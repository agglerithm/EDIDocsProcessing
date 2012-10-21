using System.Xml.Linq;

namespace EDIDocsProcessing.Common
{
    public class XMLUtilities
    {

 

        public static void set_attribute(string attribute_name, string value,
                                         XElement nde)
        {
            if (value == null) return;
            if (nde == null) return;
            if (nde.Attribute(attribute_name) == null)
            {
                nde.Add(generate_attribute(attribute_name, value));
            }
            else
                nde.Attribute(attribute_name).Value = value;
        }


        public static XAttribute generate_attribute(string name, string val)
        {
            return new XAttribute(name, val);
        }
        public static string test_attribute(string attribute_name,
                                            XElement nde)
        {
            if (nde == null) return "";
            return nde.Attribute(attribute_name) == null
                       ? "" : nde.Attribute((XName)attribute_name).Value;
        }




        public static string test_element(XElement root, string element_name)
        {
            if (root.Element(element_name) == null) return "";
            //if(root.Element(element_name).FirstNode.NodeType != System.Xml.XmlNodeType.Text) return "";
            return root.Element(element_name).Value;
        }






        public static void set_element(XElement root,
                               string element_name, string el_val)
        {
            var el = root.Element(element_name);
            if (el == null)
            {
                el = new XElement(element_name);
                root.Add(el);
            }
            if (el_val != null)
                root.SetElementValue(element_name, el_val);
        }






    }

//
//        public static XElement generate_element(string name, string val)
//        {
//            var el = new XElement(name);
//            if (val != null)
//                el.Add(new XText(val));
//            return el;
//        } 
//        public static string get_value_from_parameters(XElement parms, string name)
//        {
//            foreach (var nde in parms.Elements())
//            {
//                if (nde.Element("Name").Value == name)
//                    return nde.Element("Value").Value;
//            }
//            return "";
//        }
//
//
//        public static XElement get_text_node(string name, string val)
//        {
//            return new XElement(name, new XText(val));
//        }
//        public static XElement get_xml_from_row(DataRow dr)
//        {
//            var data_row = XElement.Parse("<dataRow />");
//            for (var i = 0; i < dr.Table.Columns.Count; i++)
//                data_row.Add(get_xml_from_column(dr, i));
//            return data_row;
//        }
//
//        public static XElement get_xml_from_column(DataRow dr, int ndx)
//        {
//            var data_column = XElement.Parse("<dataColumn />");
//            data_column.Add(new XAttribute("type", dr.Table.Columns[ndx].DataType.ToString()));
//            data_column.Add(new XAttribute("name", dr.Table.Columns[ndx].ColumnName));
//            data_column.Value = dr[ndx].ToString();
//            return data_column;
//        }
//        public static bool test_parsibility(string xml_str)
//        {
//            try
//            {
//                var x = XElement.Parse(xml_str);
//            }
//            catch
//            {
//                var test_str = "<xml_test>" + xml_str + "</xml_test>";
//                try
//                {
//                    var x = XElement.Parse(test_str);
//                }
//                catch
//                {
//                    return false;
//                }
//            }
//            return true;
//        }
    //        public static string encode(string val)
    //        {
    //            val = XmlConvert.EncodeName(val);
    //            return val;
    //        }

    //
    //        public static string get_encoded_text(XElement nde)
    //        {
    //            if (nde.FirstNode == null)
    //                return "";
    //            if (nde.FirstNode.NodeType == XmlNodeType.Text)
    //                return nde.FirstNode.ToString();
    //            return "";
    //
    //        }

    //        public static bool find_flag(XElement list, string name)
    //        {
    //            var nde = (XElement)find_in_list(list, name);
    //            return nde != null && bool.Parse(nde.Value);
    //        }

//        public static void set_boolean(XElement root, string name, bool val)
//        {
//            if (val)
//            {
//                if (root.Element(name) == null)
//                    root.Add(generate_element(name, null));
//            }
//            else
//            {
//                if (root.Elements(name) != null)
//                    root.Elements(name).Remove();
//            }
//        } 
// 

//        public static bool get_boolean(XElement root, string name)
//        {
//            return root.Element(name) != null;
//        }

 




//        public static DataTable TableFromXmlTable(XElement tbl)
//        {
//            DataTable dtbl = null;
//            string[] vals = null;
//            foreach (var row in tbl.Elements("dataRow"))
//            {
//                if (dtbl == null)
//                {
//                    dtbl = create_table_structure_from_xml(row);
//                    vals = new string[dtbl.Columns.Count];
//                }
//                dtbl.Rows.Add(populate_array_from_xml(row, vals));
//            }
//            return dtbl;
//        }

//        private static string[] populate_array_from_xml(XElement row, string[] arr)
//        {
//            if (row == null) throw new ArgumentNullException("row");
//            var ndx = 0;
//            var columns = from c in row.Elements()
//                          select c;
//            foreach (XElement column in columns)
//            {
//                arr[ndx] = get_null(column);
//
//                ndx++;
//            }
//            return arr;
//        }

        //        public static XElement get_first_element(string name, XDocument doc)
        //        {
        //            var qry = from e in doc.Elements()
        //                      where e.Name.LocalName == name
        //                      select e;
        //            if (qry == null) return null;
        //            return qry.First();
        //        }
//        private static string get_null(XElement column)
//        {
//            if (column == null) throw new ArgumentNullException("column");
//// ReSharper disable PossibleNullReferenceException
//            string type = column.Attribute("type").Value;
//// ReSharper restore PossibleNullReferenceException
//            if (column.Value != "") return column.Value;
//            if (type == "System.String") return column.Value;
//            return null;
//        }

//        private static DataTable create_table_structure_from_xml(XElement row)
//        {
//            if (row == null) throw new ArgumentNullException("row");
//            var tbl = new DataTable();
//            var columns = from c in row.Elements()
//                          select c;
//            foreach (var column in columns)
//            {
//// ReSharper disable PossibleNullReferenceException
//                tbl.Columns.Add(column.Attribute("name").Value, Type.GetType(column.Attribute("type").Value));
//// ReSharper restore PossibleNullReferenceException
//            }
//            return tbl;
//        }





 

//        public static XNode find_in_list_recursive(XElement list, string name)
//        { 
//            var qry = from r in list.Elements()
//                      select r;
//            foreach (var nde in qry)
//            {
//                if (nde.Name.LocalName == name)
//                    return nde;
//                return find_in_list_recursive(nde, name);
//            }
//            return null;
//        }

      


//        public static XAttribute get_attribute( string name, string val)
//        { 
//            return new XAttribute(name, val);  
//        }

//            public static XNode find_in_list(XElement list, string name)
//        { 
//            foreach (var nde in list.Elements())
//            {
//                if (nde.Name.LocalName == name)
//                    return nde;
//            }
//            return null;
//        }
    //        public static XElement first_element_child(XElement nde)
    //        {
    //            return nde.Elements().First();
    //        }

      
}