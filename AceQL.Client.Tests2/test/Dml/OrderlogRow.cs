using AceQL.Client.Test.Dml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AceQL.Client.test.Dml
{
	/// <summary>
	/// Simple container for orderlog table values.
	/// </summary>
    class OrderlogRow
    {
		private int customerId;
		private int itemId;
		private string description;
		private double itemCost;
		private DateTime datePlaced;
		private DateTime dateShipped;
		private String jpegImage;
		private bool isDelivered;
		private int quantity;

        public OrderlogRow()
        {
			customerId = 1;
			itemId = 11;
			description = "customer id 1 and item id 1";
			itemCost = 2000.00;
			datePlaced = DateTime.Now;
			dateShipped = DateTime.Now;
			jpegImage = AceQLTestParms.IN_DIRECTORY + "\\username_koala.jpg";
			isDelivered = true;
			quantity = 3000;
		}

		/// <returns> the customerId </returns>
		public virtual int CustomerId
		{
			get
			{
				return customerId;
			}
		}


		/// <returns> the itemId </returns>
		public virtual int ItemId
		{
			get
			{
				return itemId;
			}
		}


		/// <returns> the description </returns>
		public virtual string Description
		{
			get
			{
				return description;
			}
		}


		/// <returns> the itemCost </returns>
		public virtual Double ItemCost
		{
			get
			{
				return itemCost;
			}
		}


		/// <returns> the datePlaced </returns>
		public virtual DateTime DatePlaced
		{
			get
			{
				return datePlaced;
			}
		}


		/// <returns> the dateShipped </returns>
		public virtual DateTime DateShipped
		{
			get
			{
				return dateShipped;
			}
		}


		/// <returns> the jpegImage </returns>
		public virtual String JpegImage
		{
			get
			{
				return jpegImage;
			}
		}


		/// <returns> the isDelivered </returns>
		public virtual bool Delivered
		{
			get
			{
				return isDelivered;
			}
		}


		/// <returns> the quantity </returns>
		public virtual int Quantity
		{
			get
			{
				return quantity;
			}
		}

		public override string ToString()
        {
			return "OrderlogRow [customerId=" + customerId + ", itemId=" + itemId + ", description=" + description
				+ ", itemCost=" + itemCost + ", datePlaced=" + datePlaced + ", dateShipped=" + dateShipped
				+ ", jpegImage=" + jpegImage + ", isDelivered=" + isDelivered + ", quantity=" + quantity + "]";
		}


	}
}
