using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavaScript.Models
{
    public class PhotoViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Url { get; set; }

        public int UserId { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}