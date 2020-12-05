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

namespace Cake.MinVer
{
    /// <summary>
    /// --verbosity &lt;VERBOSITY&gt;
    /// error, warn, info (default), debug, or trace
    /// </summary>
    public enum MinVerVerbosity
    {
        /// <summary>
        /// --verbosity error
        /// </summary>
        Error,

        /// <summary>
        /// --verbosity warn
        /// </summary>
        Warn,

        /// <summary>
        /// --verbosity info
        /// </summary>
        Info,

        /// <summary>
        /// --verbosity debug
        /// </summary>
        Debug,

        /// <summary>
        /// --verbosity trace
        /// </summary>
        Trace,
    }
}
