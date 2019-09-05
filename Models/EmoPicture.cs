using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Web.Models
{
    public class EmoPicture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage="La ruta supera el tamaño establecido")]

        public string Path { get; set; }
        

        public DateTime CreateDate { get; set; } = DateTime.Now; //agregado

        public virtual ObservableCollection<EmoFace> Faces { get; set; } //No es atributo, sino algo para la navegacion

    }
}