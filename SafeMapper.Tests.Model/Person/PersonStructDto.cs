﻿namespace SafeMapper.Tests.Model.Person
{
    using System;

    public struct PersonStructDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public decimal Length { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
