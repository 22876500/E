using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QuotaShareServer.Common
{
    /// <summary>
    /// XmlOp类提供对XML数据库的读写
    /// </summary>
    public class XmlOp
    {
        //声明一个XmlDocument空对象
        protected XmlDocument XmlDoc = new XmlDocument();

        /// <summary>
        /// 构造函数，导入Xml文件
        /// </summary>
        /// <param name="xmlFile">文件虚拟路径</param>
        public XmlOp(string xmlFile)
        {
            try
            {
                XmlDoc.Load(GetXmlFilePath(xmlFile));   //载入Xml文档
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~XmlOp()
        {
            XmlDoc = null;  //释放XmlDocument对象
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filePath">文件虚拟路径</param>
        public void Save(string filePath)
        {
            try
            {
                XmlDoc.Save(GetXmlFilePath(filePath));
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回Xml文件实际路径
        /// </summary>
        /// <param name="xmlFile">文件虚拟路径</param>
        /// <returns></returns>
        public string GetXmlFilePath(string xmlFile)
        {
            string path = string.Format("{0}//{1}", System.Windows.Application.Current.StartupUri, xmlFile);
            return path;
        }

        /// <summary>
        /// 根据Xml文件的节点路径，返回一个DataSet数据集
        /// </summary>
        /// <param name="XmlPathNode">Xml文件的某个节点</param>
        /// <returns></returns>
        public DataSet GetDs(string XmlPathNode)
        {
            DataSet ds = new DataSet();
            try
            {
                System.IO.StringReader read = new System.IO.StringReader(XmlDoc.SelectSingleNode(XmlPathNode).OuterXml);
                ds.ReadXml(read);   //利用DataSet的ReadXml方法读取StringReader文件流
                read.Close();
            }
            catch (System.Exception ex)
            { }
            return ds;
        }

        /// <summary>
        /// 属性查询，返回属性值
        /// </summary>
        /// <param name="XmlPathNode">属性所在的节点</param>
        /// <param name="Attrib">属性</param>
        /// <returns></returns>
        public string SelectAttrib(string XmlPathNode, string Attrib)
        {
            string _strNode = "";
            try
            {
                _strNode = XmlDoc.SelectSingleNode(XmlPathNode).Attributes[Attrib].Value;
            }
            catch
            { }
            return _strNode;
        }

        /// <summary>
        /// 节点查询，返回节点值
        /// </summary>
        /// <param name="XmlPathNode">节点的路径</param>
        /// <returns></returns>
        public string SelectNodeText(string XmlPathNode)
        {
            string _nodeTxt = XmlDoc.SelectSingleNode(XmlPathNode).InnerText;
            if (_nodeTxt == null || _nodeTxt == "")
                return "";
            else
                return _nodeTxt;
        }

        /// <summary>
        /// 节点值查询判断
        /// </summary>
        /// <param name="XmlPathNode">父节点</param>
        /// <param name="index">节点索引</param>
        /// <param name="NodeText">节点值</param>
        /// <returns></returns>
        public bool SelectNode(string XmlPathNode, int index, string NodeText)
        {
            try
            {
                XmlNodeList _NodeList = XmlDoc.SelectNodes(XmlPathNode);
                //循环遍历节点，查询是否存在该节点
                for (int i = 0; i < _NodeList.Count; i++)
                {
                    if (_NodeList[i].ChildNodes[index].InnerText == NodeText)
                    {
                        return true;
                        break;
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 获取子节点个数
        /// </summary>
        /// <param name="XmlPathNode">父节点</param>
        /// <returns></returns>
        public int NodeCount(string XmlPathNode)
        {
            int i = 0;
            try
            {
                i = XmlDoc.SelectSingleNode(XmlPathNode).ChildNodes.Count;
            }
            catch
            {
                i = 0;
            }
            return i;
        }

        /// <summary>
        /// 更新一个节点的内容
        /// </summary>
        /// <param name="XmlPathNode">节点的路径</param>
        /// <param name="Content">新的节点值</param>
        /// <returns></returns>
        public bool UpdateNode(string XmlPathNode, string NodeContent)
        {
            try
            {
                XmlDoc.SelectSingleNode(XmlPathNode).InnerText = NodeContent;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新N个节点值
        /// </summary>
        /// <param name="XmlParentNode">父节点</param>
        /// <param name="XmlNode">子节点</param>
        /// <param name="NodeContent">子节点内容</param>
        /// <returns></returns>
        public bool UpdateNode(string[] NodeContent, string id)
        {
            try
            {
                XmlNode root = XmlDoc.SelectSingleNode("tasks");
                if (root.HasChildNodes)//如果有子节点           
                {
                    XmlNodeList _NodeList = root.ChildNodes;
                    //循环遍历节点，查询是否存在该节点
                    for (int i = 0; i < _NodeList.Count; i++)
                    {
                        if (_NodeList[i].ChildNodes[0].InnerText.Equals(id))
                        {
                            if (_NodeList[i].ChildNodes.Count != 6)
                            {
                                XmlElement objElement = XmlDoc.CreateElement("timer");
                                objElement.InnerText = "";
                                _NodeList[i].InsertAfter(objElement, _NodeList[i].ChildNodes[2]);

                                objElement = XmlDoc.CreateElement("check");
                                objElement.InnerText = "";
                                _NodeList[i].InsertAfter(objElement, _NodeList[i].ChildNodes[3]);
                            }

                            _NodeList[i].ChildNodes[1].InnerText = NodeContent[0];
                            _NodeList[i].ChildNodes[3].InnerText = NodeContent[1];
                            _NodeList[i].ChildNodes[4].InnerText = NodeContent[2];
                            break;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改属性
        /// </summary>
        /// <param name="XmlPathNode">属性所在的节点</param>
        /// <param name="Attrib">属性名</param>
        /// <param name="Content">属性值</param>
        /// <returns></returns>
        public bool UpdateAttrib(string XmlPathNode, string Attrib, string AttribContent)
        {
            try
            {
                //修改属性值
                XmlDoc.SelectSingleNode(XmlPathNode).Attributes[Attrib].Value = AttribContent;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="MainNode">属性所在节点</param>
        /// <param name="Attrib">属性名</param>
        /// <param name="AttribContent">属性值</param>
        /// <returns></returns>
        public bool InsertAttrib(string MainNode, string Attrib, string AttribContent)
        {
            try
            {
                XmlElement objNode = (XmlElement)XmlDoc.SelectSingleNode(MainNode); //强制转化成XmlElement对象
                objNode.SetAttribute(Attrib, AttribContent);    //XmlElement对象添加属性方法    
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 插入一个节点，带N个子节点
        /// </summary>
        /// <param name="MainNode">插入节点的父节点</param>
        /// <param name="ChildNode">插入节点的元素名</param>
        /// <param name="Element">插入节点的子节点名数组</param>
        /// <param name="Content">插入节点的子节点内容数组</param>
        /// <returns></returns>
        public bool InsertNode(string MainNode, string ChildNode, string[] Element, string[] Content)
        {
            try
            {
                XmlNode objRootNode = XmlDoc.SelectSingleNode(MainNode);    //声明XmlNode对象
                XmlElement objChildNode = XmlDoc.CreateElement(ChildNode);  //创建XmlElement对象
                objRootNode.AppendChild(objChildNode);
                for (int i = 0; i < Element.Length; i++)    //循环插入节点元素
                {
                    XmlElement objElement = XmlDoc.CreateElement(Element[i]);
                    objElement.InnerText = Content[i];
                    objChildNode.AppendChild(objElement);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="Node">要删除的节点</param>
        /// <returns></returns>
        public bool DeleteNode(string Node, string Id)
        {
            try
            {

                //XmlNodeList xnl = XmlDoc.SelectSingleNode("tasks").ChildNodes; //查找节点  

                //foreach (XmlNode xn in xnl)
                //{
                //    XmlElement xe = (XmlElement)xn;
                //    if (xe.InnerText.IndexOf(Node, 0) >= 0)
                //    {
                //        xn.ParentNode.RemoveChild(xn);
                //        // xn.RemoveAll();  
                //    }
                //}  
                ////XmlNode的RemoveChild方法来删除节点及其所有子节点
                //XmlDoc.SelectSingleNode(Node).ParentNode.RemoveChild(XmlDoc.SelectSingleNode(Node));
                XmlNode root = XmlDoc.SelectSingleNode("tasks");
                if (root.HasChildNodes)//如果有子节点           
                {
                    XmlNodeList nodelist = root.ChildNodes;
                    for (int i = 0; i < nodelist.Count; i++)
                    {
                        if (nodelist[i].Name.Equals(Node) && nodelist[i].ChildNodes[0].InnerText.Equals(Id))
                        {
                            nodelist[i].ParentNode.RemoveChild(nodelist[i]);
                            XmlDoc.Save("task.xml");
                            break;
                        }
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        public void DeleteXml(string nodeTag)
        {
            XmlNode root = XmlDoc.SelectSingleNode("tasks");
            if (root.HasChildNodes)//如果有子节点           
            {
                XmlNodeList nodelist = root.ChildNodes;
                for (int i = 0; i < nodelist.Count; i++)
                {
                    if (nodelist[i].Name == nodeTag)
                    {
                        nodelist[i].ParentNode.RemoveChild(nodelist[i]);
                    }
                }
                XmlDoc.Save("task.xml");
            }
        }
    }
}
