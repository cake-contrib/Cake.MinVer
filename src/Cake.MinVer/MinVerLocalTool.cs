// Copyright 2020 C. Augusto Proiete & Contributors
//
// Licensed under the MIT (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://opensource.org/licenses/MIT
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.MinVer
{
    internal class MinVerLocalTool : MinVerToolBase, IMinVerLocalTool
    {
        public MinVerLocalTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator tools, ICakeLog log)
            : base(fileSystem, environment, processRunner, tools, log)
        {
        }

        /// <inheritdoc />
        protected override ProcessArgumentBuilder GetArguments(MinVerSettings settings)
        {
            var command = new ProcessArgumentBuilder();
            var args = CreateArgumentBuilder(settings);

            command.Append("minver");

            if (!args.IsNullOrEmpty())
            {
                args.CopyTo(command);
            }

            CakeLog.Verbose("{0} arguments: [{1}]", GetToolName(), args.RenderSafe());

            return command;
        }

        /// <inheritdoc />
        protected override string GetToolName()
        {
            return "MinVer Local Tool (dotnet minver)";
        }
    }
}
