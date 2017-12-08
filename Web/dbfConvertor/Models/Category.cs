using System;
using System.Collections.Generic;

namespace ExcelToDbfConvertor.Models
{
    public partial class Category
    {
        public Category()
        {
            //Post = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }

        //public virtual ICollection<Post> Post { get; set; }
    }
}
