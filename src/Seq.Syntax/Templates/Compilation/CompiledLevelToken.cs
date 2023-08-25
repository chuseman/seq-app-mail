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

using System.IO;
using Seq.Syntax.Expressions;
using Seq.Syntax.Templates.Rendering;
using Serilog.Parsing;

namespace Seq.Syntax.Templates.Compilation;

class CompiledLevelToken : CompiledTemplate
{
    readonly string? _format;
    readonly Alignment? _alignment;

    public CompiledLevelToken(string? format, Alignment? alignment)
    {
        _format = format;
        _alignment = alignment;
    }

    public override void Evaluate(EvaluationContext ctx, TextWriter output)
    {
        if (_alignment == null)
        {
            EvaluateUnaligned(ctx, output);
        }
        else
        {
            var writer = new StringWriter();
            EvaluateUnaligned(ctx, writer);
            Padding.Apply(output, writer.ToString(), _alignment.Value);
        }
    }

    void EvaluateUnaligned(EvaluationContext ctx, TextWriter output)
    {
        output.Write(LevelRenderer.GetLevelMoniker(ctx.LogEvent.Level, _format));
    }
}