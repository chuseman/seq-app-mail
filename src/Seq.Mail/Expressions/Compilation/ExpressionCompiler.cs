﻿// Copyright © Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Globalization;
using Seq.Mail.Expressions.Ast;
using Seq.Mail.Expressions.Compilation.Arrays;
using Seq.Mail.Expressions.Compilation.Linq;
using Seq.Mail.Expressions.Compilation.Properties;
using Seq.Mail.Expressions.Compilation.Text;
using Seq.Mail.Expressions.Compilation.Variadics;
using Seq.Mail.Expressions.Compilation.Wildcards;

namespace Seq.Mail.Expressions.Compilation
{
    static class ExpressionCompiler
    {
        public static Expression Translate(Expression expression)
        {
            var actual = expression;
            actual = VariadicCallRewriter.Rewrite(actual);
            actual = TextMatchingTransformer.Rewrite(actual);
            actual = LikeSyntaxTransformer.Rewrite(actual);
            actual = PropertiesObjectAccessorTransformer.Rewrite(actual);
            actual = ConstantArrayEvaluator.Evaluate(actual);
            actual = WildcardComprehensionTransformer.Expand(actual);
            return actual;
        }

        public static Evaluatable Compile(Expression expression, CultureInfo? formatProvider,
            NameResolver nameResolver)
        {
            var actual = Translate(expression);
            return LinqExpressionCompiler.Compile(actual, formatProvider, nameResolver);
        }
    }
}
