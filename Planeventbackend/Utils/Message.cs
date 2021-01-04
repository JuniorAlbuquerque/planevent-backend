using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Utils
{
    public class Message
    {
   
        public object GetMessage(string typeMessage, string text)
        {
            if (typeMessage == "Error")
            {
                return (new {
                    error = new
                    {
                        message = text
                    }
                });

            }
            if (typeMessage == "Success")
            {
                return (new
                {
                    success = new
                    {
                        message = text
                    }
                });
            }
            return (new {
                error = new
                {
                    message = "Erro desconhecido"
                }
            });
        }
    }
}
