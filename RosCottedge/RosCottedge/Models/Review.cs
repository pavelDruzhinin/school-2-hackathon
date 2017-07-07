using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string Rating { get; set; }
        //CommentDate - дата, когда оставлен комментарий.
        public string CommentDate { get; set; }
        //Временно отключено из-за ошибки.
        //public int UserId { get; set; }
        public int HouseId { get; set; }
        public User User { get; set; }
        public House House { get; set; }
    }
}