using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mart.InstanceClasses;
using System.Windows.Forms;

namespace Mart.InstanceClasses
{
    public delegate void CreatedHandler(Employee emp);
    public delegate void UpdatedHandler(Employee emp);
    public delegate void LoadedHandler(Employee emp);
   
    public class Employee
    {
        public static event CreatedHandler Created = null;
        public static event UpdatedHandler Updated = null;
        public static event LoadedHandler Loaded = null;

        public static void LoadedInstance(int id, string firstName, string lastName, string gender, DateTime birthDate, string userName, string password, Role role, bool status, byte[] photo)
        {
            Employee emp = new Employee(id, firstName, lastName, gender, birthDate, userName, password, role, status, photo);
            if (Loaded != null) Loaded(emp);
        }

        public static void CreatedInstance(int id, string firstName, string lastName, string gender, DateTime birthDate, string userName, string password, Role role, bool status, byte[] photo)
        {
            Employee emp = new Employee(id,firstName,lastName,gender, birthDate, userName,password,role,status,photo);
            if (Created != null) Created(emp);       
        }

        public void SetEmployeeData(int id, string firstName, string lastName, string gender, DateTime birthDate, string userName, string password, Role role, bool status, byte[] photo)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.UserName = userName;
            this.BirthDate = birthDate;
            this.Password = password;
            this.Roles = role;
            this.Status = status;
            this.Photo = photo;
            if (Updated != null) Updated(this);
        }


        private Role role;
        public Role Roles
        {
            get{
                if (role != null) return role;
                else return new Role(0,"");
            }
            set{
                if (value != null) role = value;
            }
        }
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }        
        public bool Status { get; set; }

        public byte[] Photo { get; set; }

        public object Tag { get; set; }

        private Employee(int id,string firstName,string lastName,string gender, DateTime birthDate, string userName, string password,Role role,bool status, byte[] photo){
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.BirthDate = birthDate;
            this.UserName = userName;
            this.Password = password;
            this.Roles = role;
            this.Status = status;
            this.Photo = photo;
        }
    }
}
