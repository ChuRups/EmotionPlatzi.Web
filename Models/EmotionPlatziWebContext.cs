using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Web.Models
{
    public class EmotionPlatziWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public EmotionPlatziWebContext() : base("name=EmotionPlatziSql")
        {
            Database.SetInitializer<EmotionPlatziWebContext>( 
                new DropCreateDatabaseIfModelChanges<EmotionPlatziWebContext>() //Borre y cree la bdd si es que cambia (osea esto sirve para desarrollo, no cuando está en produccion porque borra la bdd)
                );  //MigrateDatabaseToLatestVersion --> cuando está en produccion, pero requiere de más cosas.
        }

        public DbSet<EmoPicture> EmoPictures { get; set; }

        public DbSet<EmoFace> EmoFaces { get; set; }

        public DbSet<EmoEmotion> EmoEmotions { get; set; }

        public System.Data.Entity.DbSet<EmotionPlatzi.Web.Models.Home> Homes { get; set; }
    }
}
