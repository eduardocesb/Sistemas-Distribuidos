using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.SaveEmail
{
    public interface ISaveEmailHandler
    {
        public void Handle(SaveEmailCommand command);
    }
}
