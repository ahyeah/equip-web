using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.FileManagment
{
    public class CatalogTreeNode
    {
        //名称
        public string text;

        //
        public Boolean selectable;

        //子节点
        public List<CatalogTreeNode> nodes = new List<CatalogTreeNode>();

        //标识
        public List<string> tags = new List<string>(); 
    }
}
