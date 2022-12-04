#region Copyright 2020-2022 C. Augusto Proiete & Contributors
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
//
#endregion

using System.Threading.Tasks;
using PublicApiGenerator;
using VerifyXunit;
using Xunit;

namespace Cake.MinVer.ApprovalTests;

/// <summary>
/// Tests for checking changes to the public API.
/// </summary>
[UsesVerify]
public class PublicApiApprovalTests
{
    /// <summary>Check for changes to the public APIs.</summary>
    [Fact]
    public async Task PublicApi_Should_Not_Change_Unintentionally()
    {
        var publicApi = typeof(MinVerTool).Assembly.GeneratePublicApi(new ApiGeneratorOptions
        {
            IncludeAssemblyAttributes = false,
            ExcludeAttributes = new[] { "System.Diagnostics.DebuggerDisplayAttribute" },
        });

        await Verifier.Verify(publicApi);
    }
}
