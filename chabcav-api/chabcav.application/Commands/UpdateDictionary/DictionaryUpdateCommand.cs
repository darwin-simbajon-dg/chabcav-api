using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace chabcav.application.Commands.UpdateDictionary
{
    public class DictionaryUpdateCommand : IRequest<bool>
    {
       // public int Id { get; set; } // ID of the dictionary file or entry
        public string UpdatedHtml { get; set; } = string.Empty; // New HTML content
    }

}
