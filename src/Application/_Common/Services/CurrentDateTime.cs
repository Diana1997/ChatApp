using System;
using Application._Common.Interfaces;

namespace Application._Common.Services
{
    public class CurrentDateTime : ICurrentDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}