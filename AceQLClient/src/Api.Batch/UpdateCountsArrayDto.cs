using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api.Batch
{
    /// <summary>
    /// Class UpdateCountsArrayDto.
    /// </summary>
    public class UpdateCountsArrayDto
    {
        private String status = "OK";
        private int[] updateCountsArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCountsArrayDto"/> class.
        /// </summary>
        /// <param name="updateCountsArray">The update counts array.</param>
        public UpdateCountsArrayDto(int[] updateCountsArray)
        {
            this.updateCountsArray = updateCountsArray;
        }

        /// <summary>
        /// Gets the update counts array.
        /// </summary>
        /// <returns>System.Int32[].</returns>
        public int[] GetUpdateCountsArray()
        {
            return updateCountsArray;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "UpdateCountsArrayDto [updateCountsArray=" + updateCountsArray + "]";
        }
    }
}
