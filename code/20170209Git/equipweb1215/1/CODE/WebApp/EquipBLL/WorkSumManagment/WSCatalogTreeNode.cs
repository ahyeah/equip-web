using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.FileManagment
{
    public class WSCatalogTreeNode
    {
        //名称
        public string text;

        //
        public Boolean selectable;

        //子节点
        public List<WSCatalogTreeNode> nodes = new List<WSCatalogTreeNode>();

        //标识
        public List<string> tags = new List<string>();
    }
}
