using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mart.InstanceClasses
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Role() { }
        public Role(int id,string name) {
            this.ID = id;
            this.Name = name;
        }

        public Role ModifyRole(int id, string name)
        {
            this.ID = id;
            this.Name = name;
            return this;
        }

    }
}
