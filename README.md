SafeMapper
=============

The goal with SafeMapper is to have an exception free and extremly fast mapper between primitive and complex types.

The project is highly influenced by AutoMapper, EmitMapper and FastMapper.

##Key features

- High speed performance
- Map between primitive types
- Map to and from string 
- Map array and collections
- Exception free conversion between most types, fallback to default value for the type
- Support for Structs and Classes
- Support for public Fields and Properties
- Support for IEnumerable<T>, IList<T>, ICollection<>T etc

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

    SafeMapper:   12.6 ms
    EmitMapper:   18.8 ms
    FastMapper:  139.8 ms
    AutoMapper: 2088.8 ms

##Planned features

- Support for circular referencies between objects
- Mapping support to and from Dictionary<sting, T> and NameValueCollection
- Support for Enums
- Configuration support to enable mapping fields/properties with different names
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