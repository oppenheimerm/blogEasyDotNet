
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BE.Core
{
    public class PostTag
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        public string? TagNameEncoded { get; set; }

        [DataMember]
        public string? TagName { get; set; }

        public Post? Post { get; set; }
    }
}
