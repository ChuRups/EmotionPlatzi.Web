using EmotionPlatzi.Web.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace EmotionPlatzi.Web.Util
{
    public class EmotionHelper
    {
        public EmotionServiceClient emoClient;

        public EmotionHelper(string key)
        {
            emoClient = new EmotionServiceClient(key);
        }

        public async Task<EmoPicture> DetectAndExtracFacesAsync(Stream imageStream) //async void ---> pero como retorna un EmoPicture debe ir dentro de un Task
        {
            Emotion[] emotions = await emoClient.RecognizeAsync(imageStream);

            var emoPicture = new EmoPicture();
            emoPicture.Faces = ExtractFaces(emotions, emoPicture);
            return emoPicture;
        }

        private ObservableCollection<EmoFace> ExtractFaces(Emotion[] emotions, EmoPicture emoPicture)
        {
            var listaFaces = new ObservableCollection<EmoFace>(); //ObservableCollection, porque es un tipo especial que emite notificaciones cada vez que se le agregan o eliminan miembros y esto sirve al EF para saber que hacer a continuacion.

            foreach (var emotion in emotions)
            {
                var emoface = new EmoFace()
                {
                    X = emotion.FaceRectangle.Left,
                    Y = emotion.FaceRectangle.Top,
                    Width = emotion.FaceRectangle.Width,
                    Height = emotion.FaceRectangle.Height,
                    Picture = emoPicture
                };
                emoface.Emotions = ProcessEmotions(emotion.Scores, emoface);
                listaFaces.Add(emoface);
            }
            return listaFaces;
        }

        private ObservableCollection<EmoEmotion> ProcessEmotions(EmotionScores scores, EmoFace emoface)
        {
            //C# es un lenguaje capaz de autodescribirse, es capaz de describir cada uno de los objetos que tiene, sabe como estan construidos cada uno de esos objetos(REFLECTION).
            var emotionList = new ObservableCollection<EmoEmotion>();
            var properties = scores.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance); //devuelve toda la informacion del tipo de datos que tiene esta variable
            var filterProperties = properties.Where(p => p.PropertyType == typeof(float)); //En toda Coleccion se puede meter Linq. | Quiero todas las propiedades de tipo float.
                                                                                           //from p in properties where p.PropertyType == typeof(float) select p;

            var emotype = EmoEmotionEnum.Undetermined; //variable auxiliar
            foreach (var prop in filterProperties)
            {
                if (!Enum.TryParse<EmoEmotionEnum>(prop.Name, out emotype))
                    emotype = EmoEmotionEnum.Undetermined;

                var emoEmotion = new EmoEmotion();
                emoEmotion.Score = (float)prop.GetValue(scores);
                emoEmotion.EmotionType = emotype;
                emoEmotion.Face = emoface;
                emotionList.Add(emoEmotion);
            }

            return emotionList;
        }






    }
}