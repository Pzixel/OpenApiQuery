using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenApiQuery.Test.Serialization.SystemText
{
    [TestClass]
    public class ApplyResultSerializationTests : SerializationTestBase
    {
        [TestMethod]
        public void TestSerialize_Simple()
        {
            var actual = new OpenApiQueryResult<SimpleClass>(2,
                new[]
                {
                    new SimpleClass
                    {
                        IntProp = 1,
                        DoubleProp = 47.11,
                        StringProp = "Hello World"
                    },
                    new SimpleClass
                    {
                        IntProp = 2,
                        DoubleProp = 47.12,
                        StringProp = "Foo Bar"
                    }
                });
            VerifySerialize(new Dictionary<string, object>
                {
                    ["@odata.count"] = 2,
                    ["value"] = new[]
                    {
                        new
                        {
                            intProp = 1,
                            doubleProp = 47.11,
                            stringProp = "Hello World"
                        },
                        new
                        {
                            intProp = 2,
                            doubleProp = 47.12,
                            stringProp = "Foo Bar"
                        }
                    }
                },
                actual);
        }

        [TestMethod]
        public void TestSerialize_Polymorphic()
        {
            var actual = new OpenApiQueryResult<Base>
            (2,
                new Base[]
                {
                    new Sub1
                    {
                        BaseProp = 1,
                        Sub1Prop = 47.11,
                        SubProp = 47
                    },
                    new Sub2
                    {
                        BaseProp = 2,
                        Sub2Prop = "Test",
                        SubProp = -47
                    }
                }
            );
            VerifySerialize(new Dictionary<string, object>
                {
                    ["@odata.count"] = 2,
                    ["value"] = new[]
                    {
                        new Dictionary<string, object>
                        {
                            ["@odata.type"] = "Sub1",
                            ["baseProp"] = 1,
                            ["sub1Prop"] = 47.11,
                            ["subProp"] = 47
                        },
                        new Dictionary<string, object>
                        {
                            ["@odata.type"] = "Sub2",
                            ["baseProp"] = 2,
                            ["sub2Prop"] = "Test",
                            ["subProp"] = -47
                        }
                    }
                },
                actual);
        }

        [TestMethod]
        public void TestSerialize_ObjectArrays()
        {
            var actual = new OpenApiQueryResult<ArrayWrapper<SimpleClass>>
            (4,
                new[]
                {
                    new ArrayWrapper<SimpleClass>
                    {
                        Items = new[]
                        {
                            new SimpleClass
                            {
                                IntProp = 1,
                                DoubleProp = 47.11,
                                StringProp = "A"
                            },
                            new SimpleClass
                            {
                                IntProp = 2,
                                DoubleProp = 47.12,
                                StringProp = "B"
                            }
                        }
                    },
                    new ArrayWrapper<SimpleClass>
                    {
                        Items = new[]
                        {
                            new SimpleClass
                            {
                                IntProp = 3,
                                DoubleProp = 47.13,
                                StringProp = "C"
                            },
                            new SimpleClass
                            {
                                IntProp = 4,
                                DoubleProp = 47.14,
                                StringProp = "D"
                            }
                        }
                    }
                }
            );
            VerifySerialize(new Dictionary<string, object>
                {
                    ["@odata.count"] = 4,
                    ["value"] = new[]
                    {
                        new
                        {
                            items = new[]
                            {
                                new
                                {
                                    intProp = 1,
                                    doubleProp = 47.11,
                                    stringProp = "A"
                                },
                                new
                                {
                                    intProp = 2,
                                    doubleProp = 47.12,
                                    stringProp = "B"
                                }
                            }
                        },
                        new
                        {
                            items = new[]
                            {
                                new
                                {
                                    intProp = 3,
                                    doubleProp = 47.13,
                                    stringProp = "C"
                                },
                                new
                                {
                                    intProp = 4,
                                    doubleProp = 47.14,
                                    stringProp = "D"
                                }
                            }
                        }
                    }
                },
                actual);
        }

        [TestMethod]
        public void TestSerialize_ObjectArrays_Polymorphic()
        {
            var actual = new OpenApiQueryResult<ArrayWrapper<Base>>
            (4,
                new[]
                {
                    new ArrayWrapper<Base>
                    {
                        Items = new Base[]
                        {
                            new Sub1
                            {
                                BaseProp = 1,
                                Sub1Prop = 47.11,
                                SubProp = 47
                            },
                            new Sub2
                            {
                                BaseProp = 2,
                                Sub2Prop = "Test",
                                SubProp = -47
                            }
                        }
                    },
                    new ArrayWrapper<Base>
                    {
                        Items = new Base[]
                        {
                            new Sub2
                            {
                                BaseProp = 3,
                                Sub2Prop = "A",
                                SubProp = -11
                            },
                            new Sub1
                            {
                                BaseProp = 4,
                                Sub1Prop = 47.14,
                                SubProp = 12
                            }
                        }
                    },
                }
            );
            VerifySerialize(new Dictionary<string, object>
                {
                    ["@odata.count"] = 4,
                    ["value"] = new[]
                    {
                        new
                        {
                            items = new object[]
                            {
                                new Dictionary<string, object>
                                {
                                    ["@odata.type"] = "Sub1",
                                    ["baseProp"] = 1,
                                    ["sub1Prop"] = 47.11,
                                    ["subProp"] = 47
                                },
                                new Dictionary<string, object>
                                {
                                    ["@odata.type"] = "Sub2",
                                    ["baseProp"] = 2,
                                    ["sub2Prop"] = "Test",
                                    ["subProp"] = -47
                                }
                            }
                        },
                        new
                        {
                            items = new object[]
                            {
                                new Dictionary<string, object>
                                {
                                    ["@odata.type"] = "Sub2",
                                    ["baseProp"] = 3,
                                    ["sub2Prop"] = "A",
                                    ["subProp"] = -11
                                },
                                new Dictionary<string, object>
                                {
                                    ["@odata.type"] = "Sub1",
                                    ["baseProp"] = 4,
                                    ["sub1Prop"] = 47.14,
                                    ["subProp"] = 12
                                }
                            }
                        }
                    }
                },
                actual);
        }

        [TestMethod]
        public void TestSerialize_NativeArrays()
        {
            TestSerialize_NativeArrays(new[]
            {
                new[]
                {
                    1, 2, 3
                },
                new[]
                {
                    4, 5, 6
                },
            });
            TestSerialize_NativeArrays(new[]
            {
                new[]
                {
                    1.0, 2.1, 3.2
                },
                new[]
                {
                    4.3, 5.4, 6.5
                },
            });
            TestSerialize_NativeArrays(new[]
            {
                new[]
                {
                    "A", "B", "C"
                },
                new[]
                {
                    "D", "E", "F"
                },
            });
        }

        public void TestSerialize_NativeArrays<T>(T[][] items)
        {
            var actual = new OpenApiQueryResult<ArrayWrapper<T>>
            (4,
                items.Select(i => new ArrayWrapper<T>
                {
                    Items = i
                }).ToArray()
            );
            VerifySerialize(new Dictionary<string, object>
                {
                    ["@odata.count"] = 4,
                    ["value"] = items.Select(i => new
                    {
                        items = i
                    }).ToArray()
                },
                actual);
        }

        [TestMethod]
        public void TestSerialize_Dictionary_SimpleTypes()
        {
            var actual = new OpenApiQueryResult<Dictionary<string, int>>
            (2,
                new[]
                {
                    new Dictionary<string, int>
                    {
                        ["A"] = 1,
                        ["b"] = 2,
                        ["C"] = 3
                    },
                    new Dictionary<string, int>
                    {
                        ["d"] = 4,
                        ["E"] = 5,
                        ["f"] = 6
                    }
                }
            );
            VerifySerialize(new Dictionary<string, object>
                {
                    ["@odata.count"] = 2,
                    ["value"] = new object[]
                    {
                        new Dictionary<string, int>
                        {
                            ["A"] = 1,
                            ["b"] = 2,
                            ["C"] = 3
                        },
                        new Dictionary<string, int>
                        {
                            ["d"] = 4,
                            ["E"] = 5,
                            ["f"] = 6
                        }
                    }
                },
                actual);
        }
    }
}
