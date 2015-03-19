SafeMapper
=============

[![Build status](https://ci.appveyor.com/api/projects/status/skkynthpqot0bmo4?svg=true)](https://ci.appveyor.com/project/unger/safemapper) [![Coverage Status](https://coveralls.io/repos/unger/SafeMapper/badge.svg)](https://coveralls.io/r/unger/SafeMapper)

The goal with SafeMapper is to have an exception free and extremly fast mapper between primitive and complex types.

The project is influenced by AutoMapper, EmitMapper and FastMapper.

##Key features

- High speed performance
- Map between primitive types and string
- Map array and collections
- Exception free conversion between most types, fallback to default value for the type
- Support for both Structs and Classes
- Support for nested objects
- Support for public Fields and Properties
- Support for interfaces IEnumerable<T>, IList<T>, ICollection<T> etc
- Enum conversion to and from int and string
- Support for Enums with DisplayAttribute/DescriptionAttribute
- Conversion from NameValueCollection to Object
- Conversion from Object to NameValueCollection
- Mapping support to and from IDictionary&lt;string, T&gt;
- Simple circular reference check on type-level
- Configuration support to enable mapping fields/properties with different names

##Installation

http://nuget.org/packages/SafeMapper/

    Install-Package SafeMapper

##Usage

The fastest way is to get a converter delegate and reuse that if you should convert many objects

    var person = new Person();
    var converter = SafeMap.GetConverter<Person, PersonDto>();
    var personDto = converter(person);

But it is also possible to call Convert directly 

    var person = new Person();
    var personDto = SafeMap.Convert<Person, PersonDto>(person);

##Speed test

Converting 100000 objects between CustomerDto and Customer, see SafeMapper.Tests.Model for definition

    SafeMapper:     13.7 ms
    EmitMapper:     17.0 ms
    FastMapper:    142.1 ms
    ValueInjector: 337.3 ms
    AutoMapper:   2092.0 ms

[More speed tests](SPEEDTESTS.md)

##Planned features

- Full Support for circular references between objects
- Support for flattening
- Plugin support to extend core functionality (Converters and Membermapping etc)
- Map from Database queries to c# object
- Map Lucene documents to c# object

##License

    Copyright (C) 2015 Magnus Unger
    
    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
    documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
    and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    The above copyright notice and this permission notice shall be included in all copies or substantial portions 
    of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
    CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
    IN THE SOFTWARE.
