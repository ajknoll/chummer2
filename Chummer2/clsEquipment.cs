﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace Chummer
{
	/// <summary>
	/// Capacity Style.
	/// </summary>
	public enum CapacityStyle
	{
		Zero = 0,
		Standard = 1,
		PerRating = 2,
	}

	public abstract class Equipment
	{
		public List<Equipment> Armors = new List<Equipment>();
		public List<Equipment> ArmorMods = new List<Equipment>();
		public List<Equipment> Cyberwares = new List<Equipment>();
		public List<Equipment> Gears = new List<Equipment>();
		public List<Equipment> Vehicles = new List<Equipment>();
		public List<Equipment> VehicleMods = new List<Equipment>();
		public List<Equipment> Weapons = new List<Equipment>();
		public List<Equipment> WeaponAccessories = new List<Equipment>();
		public List<Equipment> WeaponMods = new List<Equipment>();
		public Equipment Parent = null;

		protected Guid _guiID = new Guid();
        protected Guid _guidExternalID = new Guid();
		protected string _strName = "";
		protected string _strCategory = "";
		protected string _strNotes = "";
		protected string _strAltName = "";
		protected string _strAltCategory = "";
		protected string _strAltPage = "";
		protected string _strAvail = "";
		protected string _strCost = "";
		protected string _strSource = "";
		protected string _strPage = "";
		protected string _strExtra = "";
		protected string _strGivenName = "";
		protected int _intRating = 0;
		protected int _intMinRating = 0;
		protected int _intMaxRating = 9999;
		protected int _intQty = 1;
		protected string _strAccuracy = "0";
		protected XmlNode _nodBonus;
        protected XmlNode _nodConditional;
		protected readonly Character _objCharacter;
		protected bool _blnIncludedInParent = false;
		protected bool _blnEquipped = true;
		protected bool _blnDiscountCost = false;
		protected bool _blnWirelessCapable = true;
		protected bool _blnWirelessEnabled = true;

		//protected abstract void Create();
		public abstract void Load(XmlNode objNode, bool blnCopy = false);
		public abstract void Print(XmlTextWriter objWriter);
		public abstract void Save(XmlTextWriter objWriter);

		public Equipment(Character objCharacter)
		{
			_guiID = Guid.NewGuid();
			_objCharacter = objCharacter;
		}

		#region Simple Properties
        /// <summary>
        /// External identifier which identifies the item in the XML data file.
        /// </summary>
        public string ExternalId
        {
            get
            {
                return _guidExternalID.ToString();
            }
        }

		/// <summary>
		/// Internal identifier which will be used to identify the Equpiment in the Improvement system.
		/// </summary>
		public string InternalId
		{
			get
			{
				return _guiID.ToString();
			}
		}

		/// <summary>
		/// Name of the Equipment.
		/// </summary>
		public string Name
		{
			get
			{
				return _strName;
			}
			set
			{
				_strName = value;
			}
		}

		/// <summary>
		/// Equipment's Category.
		/// </summary>
		public string Category
		{
			get
			{
				return _strCategory;
			}
			set
			{
				_strCategory = value;
			}
		}

		/// <summary>
		/// Name that the player has assigned to the Equipment, such as a personalized name for a Weapon or piece of Armor.
		/// </summary>
		public string GivenName
		{
			get
			{
				return _strGivenName;
			}
			set
			{
				_strGivenName = value;
			}
		}

		/// <summary>
		/// Availability.
		/// </summary>
		public string Avail
		{
			get
			{
				return _strAvail;
			}
			set
			{
				_strAvail = value;
			}
		}

		/// <summary>
		/// Sourcebook.
		/// </summary>
		public string Source
		{
			get
			{
				return _strSource;
			}
			set
			{
				_strSource = value;
			}
		}

		/// <summary>
		/// Sourcebook Page Number.
		/// </summary>
		public string Page
		{
			get
			{
				string strReturn = _strPage;
				if (_strAltPage != string.Empty)
					strReturn = _strAltPage;

				return strReturn;
			}
			set
			{
				_strPage = value;
			}
		}

		/// <summary>
		/// Notes.
		/// </summary>
		public string Notes
		{
			get
			{
				return _strNotes;
			}
			set
			{
				_strNotes = value;
			}
		}

		/// <summary>
		/// Equipment's Rating.
		/// </summary>
		public int Rating
		{
			get
			{
				return _intRating;
			}
			set
			{
				_intRating = value;
				// Make sure we don't go outside of the Rating ranges.
				if (_intRating > _intMaxRating)
					_intRating = _intMaxRating;
				if (_intRating < _intMinRating)
					_intRating = _intMinRating;
			}
		}

		/// <summary>
		/// Value that was selected during an ImprovementManager dialogue.
		/// </summary>
		public string Extra
		{
			get
			{
				return _strExtra;
			}
			set
			{
				_strExtra = value;
			}
		}

		/// <summary>
		/// Minimum Rating.
		/// </summary>
		public int MinRating
		{
			get
			{
				return _intMinRating;
			}
			set
			{
				_intMinRating = value;
			}
		}

		/// <summary>
		/// Maximum Rating.
		/// </summary>
		public int MaxRating
		{
			get
			{
				return _intMaxRating;
			}
			set
			{
				_intMaxRating = value;
			}
		}

		/// <summary>
		/// Quantity.
		/// </summary>
		public int Quantity
		{
			get
			{
				return _intQty;
			}
			set
			{
				_intQty = value;
			}
		}

		/// <summary>
		/// Bonus node from the XML file.
		/// </summary>
		public XmlNode Bonus
		{
			get
			{
				return _nodBonus;
			}
			set
			{
				_nodBonus = value;
			}
		}

        /// <summary>
        /// Conditional Bonus node from the XML file.
        /// </summary>
        public XmlNode Conditional
        {
            get
            {
                return _nodConditional;
            }
            set
            {
                _nodConditional = value;
            }
        }

		/// <summary>
		/// Whether or not this Equipment is part of its parent's base configuration.
		/// </summary>
		public bool IncludedInParent
		{
			get
			{
				return _blnIncludedInParent;
			}
			set
			{
				_blnIncludedInParent = value;
			}
		}

		/// <summary>
		/// Whether or not this Equipment is currently equipped by the character or installed in another piece of Equipment.
		/// </summary>
		public bool Equipped
		{
			get
			{
				return _blnEquipped;
			}
			set
			{
				_blnEquipped = value;
			}
		}

		/// <summary>
		/// Accuracy.
		/// </summary>
		public string Accuracy
		{
			get
			{
				return _strAccuracy;
			}
			set
			{
				_strAccuracy = value;
			}
		}

		/// <summary>
		/// Whether or not the Equipment's cost should be discounted by 10% through the Black Market Pipeline Quality.
		/// </summary>
		public bool DiscountCost
		{
			get
			{
				if (_objCharacter.Created)
					return false;
				else
					return _blnDiscountCost;
			}
			set
			{
				_blnDiscountCost = value;
			}
		}

		/// <summary>
		/// Whether or not the Equipment is capabale of using Wireless mode.
		/// </summary>
		public bool WirelessCapable
		{
			get
			{
				return _blnWirelessCapable;
			}
			set
			{
				_blnWirelessCapable = value;
			}
		}

		/// <summary>
		/// Whether or not the Equipment is currently running in Wireless mode.
		/// </summary>
		public bool WirelessEnabled
		{
			get
			{
				return _blnWirelessEnabled;
			}
			set
			{
				// Naturally, Equipment that is not capable of using Wireless mode cannot enter Wireless mode.
				if (!_blnWirelessCapable)
					_blnWirelessEnabled = false;
				else
					_blnWirelessEnabled = value;
			}
		}
		#endregion

		#region Complex Properties
		/// <summary>
		/// The name of the object as it should appear on printouts (translated name only).
		/// </summary>
		public string DisplayNameShort
		{
			get
			{
				string strReturn = _strName;
				if (_strAltName != string.Empty)
					strReturn = _strAltName;

				return strReturn;
			}
		}

		/// <summary>
		/// The name of the object as it should be displayed in lists. Qty Name (Rating) (Extra).
		/// </summary>
		public string DisplayName
		{
			get
			{
				string strReturn = DisplayNameShort;

				if (_intQty > 1)
					strReturn = _intQty.ToString() + " " + strReturn;
				if (_strExtra != "")
					strReturn += " (" + LanguageManager.Instance.TranslateExtra(_strExtra) + ")";
				if (_intRating > 0)
					strReturn += " (" + LanguageManager.Instance.GetString("String_Rating") + " " + _intRating.ToString() + ")";
				if (_strGivenName != "")
					strReturn += " (\"" + _strGivenName + "\")";
				return strReturn;
			}
		}

		/// <summary>
		/// Translated Category.
		/// </summary>
		public string DisplayCategory
		{
			get
			{
				string strReturn = _strCategory;
				if (_strAltCategory != string.Empty)
					strReturn = _strAltCategory;

				// Some Categories are actually the name of object types, so pull them from the language file.
				if (strReturn == "Gear")
				{
					strReturn = LanguageManager.Instance.GetString("String_SelectPACKSKit_Gear");
				}
				if (strReturn == "Cyberware")
				{
					strReturn = LanguageManager.Instance.GetString("String_SelectPACKSKit_Cyberware");
				}
				if (strReturn == "Bioware")
				{
					strReturn = LanguageManager.Instance.GetString("String_SelectPACKSKit_Bioware");
				}

				return strReturn;
			}
		}

		/// <summary>
		/// Total Availability for the Equipment.
		/// </summary>
		public string TotalAvail
		{
			get
			{
				string strUseAvail = "";
				string strReturn = "";
				bool blnAddition = Avail.StartsWith("+");

				if (blnAddition)
					strUseAvail = Avail.Replace("+", string.Empty);
				else
					strUseAvail = Avail;

                if (strUseAvail.StartsWith("FixedValues"))
                    strUseAvail = FixedValue(strUseAvail);

				if (strUseAvail.Contains("Rating"))
					strUseAvail = strUseAvail.Replace("Rating", _intRating.ToString());

				int intAvail = 0;
				string strAvailText = "";
				if (strUseAvail.Contains("F") || strUseAvail.Contains("R"))
				{
					strAvailText = strUseAvail.Substring(strUseAvail.Length - 1);
					intAvail = Convert.ToInt32(strUseAvail.Replace(strAvailText, string.Empty));
				}
				else
					intAvail = Convert.ToInt32(strUseAvail);

				// Run through the child items and increase Avail by any plugins whose Avail contains "+".
				foreach (Armor objChild in Armors)
				{
					string strChildAvail = objChild.TotalAvail;
					if (strChildAvail.Contains("+") && !objChild.IncludedInParent)
					{
						if (strChildAvail.Contains("F"))
							strAvailText = "F";
						else if (strChildAvail.Contains("R") && strAvailText != "F")
							strAvailText = "R";

						intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
				}

				foreach (ArmorMod objChild in ArmorMods)
				{
					string strChildAvail = objChild.TotalAvail;
					if (strChildAvail.Contains("+") && !objChild.IncludedInParent)
					{
						if (strChildAvail.Contains("F"))
							strAvailText = "F";
						else if (strChildAvail.Contains("R") && strAvailText != "F")
							strAvailText = "R";

						intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
				}

				foreach (Cyberware objChild in Cyberwares)
				{
					string strChildAvail = objChild.TotalAvail;
					if (strChildAvail.Contains("+") && !objChild.IncludedInParent)
					{
						if (strChildAvail.Contains("F"))
							strAvailText = "F";
						else if (strChildAvail.Contains("R") && strAvailText != "F")
							strAvailText = "R";

						intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
				}

				foreach (Gear objChild in Gears)
				{
					string strChildAvail = objChild.TotalAvail();
					if (strChildAvail.Contains("+") && !objChild.IncludedInParent)
					{
						if (strChildAvail.Contains("F"))
							strAvailText = "F";
						else if (strChildAvail.Contains("R") && strAvailText != "F")
							strAvailText = "R";

						intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
				}

				foreach (Vehicle objChild in Vehicles)
				{
					string strChildAvail = objChild.TotalAvail;
					if (strChildAvail.Contains("+") && !objChild.IncludedInParent)
					{
						if (strChildAvail.Contains("F"))
							strAvailText = "F";
						else if (strChildAvail.Contains("R") && strAvailText != "F")
							strAvailText = "R";

						intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
				}

				foreach (VehicleMod objChild in VehicleMods)
				{
					string strChildAvail = objChild.TotalAvail;
					if (strChildAvail.Contains("+") && !objChild.IncludedInParent)
					{
						if (strChildAvail.Contains("F"))
							strAvailText = "F";
						else if (strChildAvail.Contains("R") && strAvailText != "F")
							strAvailText = "R";

						intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
				}

				foreach (Weapon objChild in Weapons)
				{
					string strChildAvail = objChild.TotalAvail;
					if (strChildAvail.Contains("+") && !objChild.IncludedInParent)
					{
						if (strChildAvail.Contains("F"))
							strAvailText = "F";
						else if (strChildAvail.Contains("R") && strAvailText != "F")
							strAvailText = "R";

						intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
				}

				foreach (WeaponAccessory objChild in WeaponAccessories)
				{
					string strChildAvail = objChild.TotalAvail;
					if (strChildAvail.Contains("+") && !objChild.IncludedInParent)
					{
						if (strChildAvail.Contains("F"))
							strAvailText = "F";
						else if (strChildAvail.Contains("R") && strAvailText != "F")
							strAvailText = "R";

						intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
				}

				foreach (WeaponMod objChild in WeaponMods)
				{
					string strChildAvail = objChild.TotalAvail;
					if (strChildAvail.Contains("+") && !objChild.IncludedInParent)
					{
						if (strChildAvail.Contains("F"))
							strAvailText = "F";
						else if (strChildAvail.Contains("R") && strAvailText != "F")
							strAvailText = "R";

						intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
				}

				strReturn = intAvail.ToString() + strAvailText;

				// Translate the Avail string.
				strReturn = strReturn.Replace("R", LanguageManager.Instance.GetString("String_AvailRestricted"));
				strReturn = strReturn.Replace("F", LanguageManager.Instance.GetString("String_AvailForbidden"));

				if (blnAddition)
					strReturn = "+" + strReturn;

				return strReturn;
			}
		}

        /// <summary>
        /// Total Accuracy for the Equipment.
        /// </summary>
        public int TotalAccuracy
        {
            get
            {
                ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);

                int intAccuracy = 0;
                try
                {
                    intAccuracy = Convert.ToInt32(_strAccuracy);
                }
                catch
                {
                    if (_strAccuracy == "Physical")
                        intAccuracy = _objCharacter.PhysicalLimit();
                    else if (_strAccuracy == "Mental")
                        intAccuracy = _objCharacter.MentalLimit();
                    else if (_strAccuracy == "Social")
                        intAccuracy = _objCharacter.SocialLimit();
                    else if (_strAccuracy == "Astral")
                        intAccuracy = _objCharacter.AstralLimit();
                }

                return intAccuracy + objImprovementManager.ValueOf(Improvement.ImprovementType.Accuracy);
            }
        }
		#endregion

        /// <summary>
        /// Prompt the user to select a cost for the items since its price is Variable.
        /// </summary>
        /// <param name="strVariableCost">Variable cost string from the Equipment.</param>
        /// <param name="strDisplayName">Name to show in the dialogue window.</param>
        protected int VariableCost(string strVariableCost, string strDisplayName)
        {
            int intMin = 0;
            int intMax = 0;
            string strCost = strVariableCost.Replace("Variable(", string.Empty).Replace(")", string.Empty);

            if (strCost.Contains("-"))
            {
                string[] strValues = strCost.Split('-');
                intMin = Convert.ToInt32(strValues[0]);
                intMax = Convert.ToInt32(strValues[1]);
            }
            else
                intMin = Convert.ToInt32(strCost.Replace("+", string.Empty));

            if (intMin != 0 || intMax != 0)
            {
                frmSelectNumber frmPickNumber = new frmSelectNumber();
                if (intMax == 0)
                    intMax = 1000000;
                frmPickNumber.Minimum = intMin;
                frmPickNumber.Maximum = intMax;
                frmPickNumber.Description = LanguageManager.Instance.GetString("String_SelectVariableCost").Replace("{0}", strDisplayName);
                frmPickNumber.AllowCancel = false;
                frmPickNumber.ShowDialog();

                return Convert.ToInt32(frmPickNumber.SelectedValue.ToString());
            }
            else
                return 0;
        }

        /// <summary>
        /// Get a FixedValues value based on the Equipment's Rating.
        /// </summary>
        /// <param name="strFixedValue">FixedValues string to parse.</param>
        public string FixedValue(string strFixedValue)
        {
            string[] strValues = strFixedValue.Replace("FixedValues(", string.Empty).Replace(")", string.Empty).Split(',');
            return strValues[_intRating - 1];
        }

		// Include common functions (not abstract) to get item's own Cost and Avail plus total (all plugins included) Cost and Avail.
	}

	/// <summary>
	/// A piece of Armor Modification.
	/// </summary>
	public class ArmorMod : Equipment
	{
		private string _strArmorCapacity = "[0]";
		private int _intArmorValue = 0;
		private Guid _guiWeaponID = new Guid();

		#region Constructor, Create, Save, Load, and Print Methods
		public ArmorMod(Character objCharacter) : base(objCharacter)
		{
			// Create the GUID for the new Armor Mod.
			_guiID = Guid.NewGuid();
		}

		/// Create a Armor Modification from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlArmorNode">XmlNode to create the object from.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		/// <param name="intRating">Rating of the selected ArmorMod.</param>
		/// <param name="objWeapons">List of Weapons that are created by the Armor.</param>
		/// <param name="objWeaponNodes">List of Weapon Nodes that are created by the Armor.</param>
		/// <param name="blnSkipCost">Whether or not creating the Armor should skip the Variable price dialogue (should only be used by frmSelectArmor).</param>
        /// <param name="blnCreateImprovements">Whether or not Improvements should be created.</param>
		public void Create(XmlNode objXmlArmorNode, TreeNode objNode, int intRating, List<Weapon> objWeapons, List<TreeNode> objWeaponNodes, bool blnSkipCost = false, bool blnCreateImprovements = true)
		{
            _guidExternalID = Guid.Parse(objXmlArmorNode["id"].InnerText);
			_strName = objXmlArmorNode["name"].InnerText;
			_strCategory = objXmlArmorNode["category"].InnerText;
			_strArmorCapacity = objXmlArmorNode["armorcapacity"].InnerText;
			_intArmorValue = Convert.ToInt32(objXmlArmorNode["armorvalue"].InnerText);
			_intRating = intRating;
			_intMaxRating = Convert.ToInt32(objXmlArmorNode["maxrating"].InnerText);
			_strAvail = objXmlArmorNode["avail"].InnerText;
			_strSource = objXmlArmorNode["source"].InnerText;
			_strPage = objXmlArmorNode["page"].InnerText;
			_nodBonus = objXmlArmorNode["bonus"];
            _nodConditional = objXmlArmorNode["conditional"];

            if (objXmlArmorNode["cost"].InnerText.StartsWith("Variable"))
            {
                if (blnSkipCost)
                    _strCost = "0";
                else
                    _strCost = VariableCost(objXmlArmorNode["cost"].InnerText, DisplayNameShort).ToString();
            }
            else
                _strCost = objXmlArmorNode["cost"].InnerText;

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("armor.xml");
				XmlNode objArmorNode = objXmlDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + ExternalId + "\"]");
				if (objArmorNode != null)
				{
					if (objArmorNode["translate"] != null)
						_strAltName = objArmorNode["translate"].InnerText;
					if (objArmorNode["altpage"] != null)
						_strAltPage = objArmorNode["altpage"].InnerText;
				}

				objArmorNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objArmorNode != null)
				{
					if (objArmorNode.Attributes["translate"] != null)
						_strAltCategory = objArmorNode.Attributes["translate"].InnerText;
				}
			}

			if (objXmlArmorNode["bonus"] != null && !blnSkipCost && blnCreateImprovements)
			{
				ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
				if (!objImprovementManager.CreateImprovements(Improvement.ImprovementSource.ArmorMod, _guiID.ToString(), objXmlArmorNode["bonus"], false, intRating, DisplayNameShort))
				{
					_guiID = Guid.Empty;
					return;
				}
				if (objImprovementManager.SelectedValue != "")
				{
					_strExtra = objImprovementManager.SelectedValue;
					objNode.Text += " (" + objImprovementManager.SelectedValue + ")";
				}
			}

			// Add Weapons if applicable.
			if (objXmlArmorNode["addweapon"] != null)
			{
				XmlDocument objXmlWeaponDocument = XmlManager.Instance.Load("weapons.xml");

				// More than one Weapon can be added, so loop through all occurrences.
				foreach (XmlNode objXmlAddWeapon in objXmlArmorNode.SelectNodes("addweapon"))
				{
					XmlNode objXmlWeapon = objXmlWeaponDocument.SelectSingleNode("/chummer/weapons/weapon[id = \"" + objXmlAddWeapon.InnerText + "\"]");

					TreeNode objGearWeaponNode = new TreeNode();
					Weapon objGearWeapon = new Weapon(_objCharacter);
					objGearWeapon.Create(objXmlWeapon, _objCharacter, objGearWeaponNode, null, null, null);
					objGearWeaponNode.ForeColor = SystemColors.GrayText;
					objWeaponNodes.Add(objGearWeaponNode);
					objWeapons.Add(objGearWeapon);

					_guiWeaponID = Guid.Parse(objGearWeapon.InternalId);
				}
			}

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("armormod");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("category", _strCategory);
			objWriter.WriteElementString("armorvalue", _intArmorValue.ToString());
			objWriter.WriteElementString("armorcapacity", _strArmorCapacity);
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("cost", _strCost);
			if (_nodBonus != null)
				objWriter.WriteRaw(_nodBonus.OuterXml);
			else
				objWriter.WriteElementString("bonus", "");
            if (_nodConditional != null)
                objWriter.WriteRaw(_nodConditional.OuterXml);
            else
                objWriter.WriteElementString("conditional", "");
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("included", _blnIncludedInParent.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("extra", _strExtra);
			if (_guiWeaponID != Guid.Empty)
				objWriter.WriteElementString("weaponguid", _guiWeaponID.ToString());
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Attribute from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public override void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_strCategory = objNode["category"].InnerText;
			_intArmorValue = Convert.ToInt32(objNode["armorvalue"].InnerText);
			_strArmorCapacity = objNode["armorcapacity"].InnerText;
			_intMaxRating = Convert.ToInt32(objNode["maxrating"].InnerText);
			_intRating = Convert.ToInt32(objNode["rating"].InnerText);
			_strAvail = objNode["avail"].InnerText;
			_strCost = objNode["cost"].InnerText;
			_nodBonus = objNode["bonus"];
            _nodConditional = objNode["conditional"];
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;
			_blnIncludedInParent = Convert.ToBoolean(objNode["included"].InnerText);
			_blnEquipped = Convert.ToBoolean(objNode["equipped"].InnerText);
			_strExtra = objNode["extra"].InnerText;
            if (objNode["weaponguid"] != null)
    			_guiWeaponID = Guid.Parse(objNode["weaponguid"].InnerText);
            _strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("armor.xml");
				XmlNode objArmorNode = objXmlDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + ExternalId + "\"]");
				if (objArmorNode != null)
				{
					if (objArmorNode["translate"] != null)
						_strAltName = objArmorNode["translate"].InnerText;
					if (objArmorNode["altpage"] != null)
						_strAltPage = objArmorNode["altpage"].InnerText;
				}

				objArmorNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objArmorNode != null)
				{
					if (objArmorNode.Attributes["translate"] != null)
						_strAltCategory = objArmorNode.Attributes["translate"].InnerText;
				}
			}

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("armormod");
			objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("name_english", _strName);
			objWriter.WriteElementString("category", DisplayCategory);
			objWriter.WriteElementString("category_english", _strCategory);
			objWriter.WriteElementString("armorvalue", _intArmorValue.ToString());
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("avail", TotalAvail);
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			objWriter.WriteElementString("included", _blnIncludedInParent.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("extra", LanguageManager.Instance.TranslateExtra(_strExtra));
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Guid of a Cyberware Weapon.
		/// </summary>
		public string WeaponID
		{
			get
			{
				return _guiWeaponID.ToString();
			}
			set
			{
				_guiWeaponID = Guid.Parse(value);
			}
		}

		/// <summary>
		/// Mod's Armor value modifier.
		/// </summary>
		public int ArmorValue
		{
			get
			{
				return _intArmorValue;
			}
			set
			{
				_intArmorValue = value;
			}
		}

		/// <summary>
		/// Armor capacity.
		/// </summary>
		public string ArmorCapacity
		{
			get
			{
				return _strArmorCapacity;
			}
			set
			{
				_strArmorCapacity = value;
			}
		}

		/// <summary>
		/// The Mod's cost.
		/// </summary>
		public string Cost
		{
			get
			{
				if (_strCost.Contains("Rating"))
				{
					// If the cost is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strCost = "";
					string strCostExpression = _strCost;

					strCost = strCostExpression.Replace("Rating", _intRating.ToString());
					XPathExpression xprCost = nav.Compile(strCost);
					return nav.Evaluate(xprCost).ToString();
				}
				else
				{
					// Just a straight cost, so return the value.
					return _strCost;
				}
			}
			set
			{
				_strCost = value;
			}
		}
		#endregion

		#region Complex Properties
		/// <summary>
		/// Caculated Capacity of the Armor Mod.
		/// </summary>
		public string CalculatedCapacity
		{
			get
			{
				if (_strArmorCapacity.Contains("/["))
				{
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					int intPos = _strArmorCapacity.IndexOf("/[");
					string strFirstHalf = _strArmorCapacity.Substring(0, intPos);
					string strSecondHalf = _strArmorCapacity.Substring(intPos + 1, _strArmorCapacity.Length - intPos - 1);
					bool blnSquareBrackets = false;
					string strCapacity = "";

					try
					{
						blnSquareBrackets = strFirstHalf.Contains('[');
						strCapacity = strFirstHalf;
						if (blnSquareBrackets)
							strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
					}
					catch
					{
					}
					XPathExpression xprCapacity = nav.Compile(strCapacity.Replace("Rating", _intRating.ToString()));

					string strReturn = "";
					try
					{
						if (_strArmorCapacity == "[*]")
							strReturn = "*";
						else
						{
							if (_strArmorCapacity.StartsWith("FixedValues"))
								strReturn = FixedValue(_strArmorCapacity);
							else
								strReturn = nav.Evaluate(xprCapacity).ToString();
						}
						if (blnSquareBrackets)
							strReturn = "[" + strCapacity + "]";
					}
					catch
					{
						strReturn = "0";
					}
					strReturn += "/" + strSecondHalf;
					return strReturn;
				}
				else if (_strArmorCapacity.Contains("Rating"))
				{
					// If the Capaicty is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					// XPathExpression cannot evaluate while there are square brackets, so remove them if necessary.
					bool blnSquareBrackets = _strArmorCapacity.Contains('[');
					string strCapacity = _strArmorCapacity;
					if (blnSquareBrackets)
						strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
					XPathExpression xprCapacity = nav.Compile(strCapacity.Replace("Rating", _intRating.ToString()));

					string strReturn = nav.Evaluate(xprCapacity).ToString();
					if (blnSquareBrackets)
						strReturn = "[" + strReturn + "]";

					return strReturn;
				}
				else
				{
					// Just a straight Capacity, so return the value.
					if (_strArmorCapacity == "")
						return "0";
					else if (_strArmorCapacity.StartsWith("FixedValues"))
						return FixedValue(_strArmorCapacity);
					else
						return _strArmorCapacity;
				}
			}
		}

		/// <summary>
		/// Total cost of the Armor Mod.
		/// </summary>
		public int TotalCost
		{
			get
			{
				int intReturn = 0;

				if (_strCost.Contains("Armor Cost"))
				{
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();
					string strCostExpr = _strCost.Replace("Armor Cost", ((Armor)Parent).Cost.ToString());
					XPathExpression xprCost = nav.Compile(strCostExpr);
					intReturn = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(nav.Evaluate(xprCost).ToString(), GlobalOptions.Instance.CultureInfo)));
				}
				else if (_strCost.Contains("Rating"))
				{
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strCostExpr = _strCost.Replace("Rating", _intRating.ToString());
					XPathExpression xprCost = nav.Compile(strCostExpr);
					intReturn = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(nav.Evaluate(xprCost).ToString(), GlobalOptions.Instance.CultureInfo)));
				}
				else
					intReturn = Convert.ToInt32(_strCost);

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				return intReturn;
			}
		}

		/// <summary>
		/// Cost for just the Armor Mod.
		/// </summary>
		public int OwnCost
		{
			get
			{
				return TotalCost;
			}
		}
		#endregion
	}

	/// <summary>
	/// A specific piece of Armor.
	/// </summary>
	public class Armor : Equipment
	{
        private string _strArmorValue = "0";
		private string _strArmorCapacity = "0";
		private int _intDamage = 0;
		protected string _strLocation = "";

		#region Constructor, Create, Save, Load, and Print Methods
		public Armor(Character objCharacter) : base(objCharacter)
		{
		}

		/// Create a Cyberware from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlArmorNode">XmlNode to create the object from.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		/// <param name="cmsArmorMod">ContextMenuStrip to apply to Armor Mode TreeNodes.</param>
		/// <param name="blnSkipCost">Whether or not creating the Armor should skip the Variable price dialogue (should only be used by frmSelectArmor).</param>
		/// <param name="blnCreateChildren">Whether or not child items should be created.</param>
        /// <param name="blnCreateImprovements">Whether or not Improvements should be created.</param>
		public void Create(XmlNode objXmlArmorNode, TreeNode objNode, ContextMenuStrip cmsArmorMod, bool blnSkipCost = false, bool blnCreateChildren = true, bool blnCreateImprovements = true)
		{
            _guidExternalID = Guid.Parse(objXmlArmorNode["id"].InnerText);
			_strName = objXmlArmorNode["name"].InnerText;
			_strCategory = objXmlArmorNode["category"].InnerText;
			_strArmorValue = objXmlArmorNode["armorvalue"].InnerText;
			_strArmorCapacity = objXmlArmorNode["armorcapacity"].InnerText;
			_strAvail = objXmlArmorNode["avail"].InnerText;
			_strSource = objXmlArmorNode["source"].InnerText;
			_strPage = objXmlArmorNode["page"].InnerText;
			_nodBonus = objXmlArmorNode["bonus"];
            _nodConditional = objXmlArmorNode["conditional"];

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("armor.xml");
				XmlNode objArmorNode = objXmlDocument.SelectSingleNode("/chummer/armors/armor[id = \"" + ExternalId + "\"]");
				if (objArmorNode != null)
				{
					if (objArmorNode["translate"] != null)
						_strAltName = objArmorNode["translate"].InnerText;
					if (objArmorNode["altpage"] != null)
						_strAltPage = objArmorNode["altpage"].InnerText;
				}

				objArmorNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objNode != null)
				{
					if (objArmorNode.Attributes["translate"] != null)
						_strAltCategory = objArmorNode.Attributes["translate"].InnerText;
				}
			}

			// Check for a Variable Cost.
			if (objXmlArmorNode["cost"].InnerText.StartsWith("Variable"))
			{
				if (blnSkipCost)
					_strCost = "0";
				else
                    _strCost = VariableCost(objXmlArmorNode["cost"].InnerText, DisplayNameShort).ToString();
			}
			else
				_strCost = objXmlArmorNode["cost"].InnerText;

			if (objXmlArmorNode["bonus"] != null && !blnSkipCost && blnCreateImprovements)
			{
				ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
				if (!objImprovementManager.CreateImprovements(Improvement.ImprovementSource.Armor, _guiID.ToString(), objXmlArmorNode["bonus"], false, 1, DisplayNameShort))
				{
					_guiID = Guid.Empty;
					return;
				}
				if (objImprovementManager.SelectedValue != "")
				{
					_strExtra = objImprovementManager.SelectedValue;
					objNode.Text += " (" + objImprovementManager.SelectedValue + ")";
				}
			}

			// Add any Armor Mods that come with the Armor.
			if (objXmlArmorNode["mods"] != null && blnCreateChildren)
			{
				XmlDocument objXmlArmorDocument = XmlManager.Instance.Load("armor.xml");

				foreach (XmlNode objXmlArmorMod in objXmlArmorNode.SelectNodes("mods/id"))
				{
					int intRating = 0;
					string strForceValue = "";
					if (objXmlArmorMod.Attributes["rating"] != null)
						intRating = Convert.ToInt32(objXmlArmorMod.Attributes["rating"].InnerText);
					if (objXmlArmorMod.Attributes["select"] != null)
						strForceValue = objXmlArmorMod.Attributes["select"].ToString();

					XmlNode objXmlMod = objXmlArmorDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + objXmlArmorMod.InnerText + "\"]");
					ArmorMod objMod = new ArmorMod(_objCharacter);
					List<Weapon> lstWeapons = new List<Weapon>();
					List<TreeNode> lstWeaponNodes = new List<TreeNode>();

					TreeNode objModNode = new TreeNode();

					objMod.Create(objXmlMod, objModNode, intRating, lstWeapons, lstWeaponNodes, blnSkipCost, blnCreateImprovements);
					objMod.Parent = this;
					objMod.IncludedInParent = true;
					objMod.ArmorCapacity = "[0]";
					objMod.Cost = "0";
					objMod.MaxRating = objMod.Rating;
					ArmorMods.Add(objMod);

					objModNode.ContextMenuStrip = cmsArmorMod;
					objNode.Nodes.Add(objModNode);
					objNode.Expand();
				}
			}

			// Add any Gear that comes with the Armor.
			if (objXmlArmorNode["gears"] != null && blnCreateChildren)
			{
				XmlDocument objXmlGearDocument = XmlManager.Instance.Load("gear.xml");
				foreach (XmlNode objXmlArmorGear in objXmlArmorNode.SelectNodes("gears/id"))
				{
					int intRating = 0;
					string strForceValue = "";
					if (objXmlArmorGear.Attributes["rating"] != null)
						intRating = Convert.ToInt32(objXmlArmorGear.Attributes["rating"].InnerText);
					if (objXmlArmorGear.Attributes["select"] != null)
						strForceValue = objXmlArmorGear.Attributes["select"].InnerText;

					XmlNode objXmlGear = objXmlGearDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + objXmlArmorGear.InnerText + "\"]");
					Gear objGear = new Gear(_objCharacter);

					TreeNode objGearNode = new TreeNode();
					List<Weapon> lstWeapons = new List<Weapon>();
					List<TreeNode> lstWeaponNodes = new List<TreeNode>();

					objGear.Create(objXmlGear, _objCharacter, objGearNode, intRating, lstWeapons, lstWeaponNodes, strForceValue, false, false, blnCreateImprovements);
					objGear.Capacity = "[0]";
					objGear.ArmorCapacity = "[0]";
					objGear.Cost = "0";
					objGear.MaxRating = objGear.Rating;
					objGear.MinRating = objGear.Rating;
					objGear.IncludedInParent = true;
					Gears.Add(objGear);

					objNode.Nodes.Add(objGearNode);
					objNode.Expand();
				}
			}

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("armor");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("category", _strCategory);
			objWriter.WriteElementString("armorvalue", _strArmorValue);
			objWriter.WriteElementString("armorcapacity", _strArmorCapacity);
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("extra", _strExtra);
			objWriter.WriteElementString("damage", _intDamage.ToString());
			objWriter.WriteStartElement("armormods");
			foreach (ArmorMod objMod in ArmorMods)
			{
				objMod.Save(objWriter);
			}
			objWriter.WriteEndElement();
			if (Gears.Count > 0)
			{
				objWriter.WriteStartElement("gears");
				foreach (Gear objGear in Gears)
				{
					// Use the Gear's SubClass if applicable.
					if (objGear.GetType() == typeof(Commlink))
					{
						Commlink objCommlink = new Commlink(_objCharacter);
						objCommlink = (Commlink)objGear;
						objCommlink.Save(objWriter);
					}
					else if (objGear.GetType() == typeof(OperatingSystem))
					{
						OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
						objOperatinSystem = (OperatingSystem)objGear;
						objOperatinSystem.Save(objWriter);
					}
					else
					{
						objGear.Save(objWriter);
					}
				}
				objWriter.WriteEndElement();
			}
			if (_nodBonus != null)
				objWriter.WriteRaw(_nodBonus.OuterXml);
			else
				objWriter.WriteElementString("bonus", "");
            if (_nodConditional != null)
                objWriter.WriteRaw(_nodConditional.OuterXml);
            else
                objWriter.WriteElementString("conditional", "");
			objWriter.WriteElementString("location", _strLocation);
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Attribute from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public override void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_strCategory = objNode["category"].InnerText;
			_strArmorValue = objNode["armorvalue"].InnerText;
			_strArmorCapacity = objNode["armorcapacity"].InnerText;
			_strAvail = objNode["avail"].InnerText;
			_strCost = objNode["cost"].InnerText;
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;
			_strGivenName = objNode["givenname"].InnerText;
			_blnEquipped = Convert.ToBoolean(objNode["equipped"].InnerText);
			_strExtra = objNode["extra"].InnerText;
			_intDamage = Convert.ToInt32(objNode["damage"].InnerText);

			if (objNode["armormods"] != null)
			{
				XmlNodeList nodMods = objNode.SelectNodes("armormods/armormod");
				foreach (XmlNode nodMod in nodMods)
				{
					ArmorMod objMod = new ArmorMod(_objCharacter);
					objMod.Load(nodMod, blnCopy);
					objMod.Parent = this;
					ArmorMods.Add(objMod);
				}
			}
			if (objNode["gears"] != null)
			{
				XmlNodeList nodGears = objNode.SelectNodes("gears/gear");
				foreach (XmlNode nodGear in nodGears)
				{
					switch (nodGear["category"].InnerText)
					{
						case "Commlink":
						case "Commlink Upgrade":
							Commlink objCommlink = new Commlink(_objCharacter);
							objCommlink.Load(nodGear, blnCopy);
							objCommlink.Parent = this;
							Gears.Add(objCommlink);
							break;
						case "Commlink Operating System":
						case "Commlink Operating System Upgrade":
							OperatingSystem objOperatingSystem = new OperatingSystem(_objCharacter);
							objOperatingSystem.Load(nodGear, blnCopy);
							objOperatingSystem.Parent = this;
							Gears.Add(objOperatingSystem);
							break;
						default:
							Gear objGear = new Gear(_objCharacter);
							objGear.Load(nodGear, blnCopy);
							objGear.Parent = this;
							Gears.Add(objGear);
							break;
					}
				}
			}

			_nodBonus = objNode["bonus"];
            _nodConditional = objNode["conditional"];
			_strLocation = objNode["location"].InnerText;
			_strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlArmorDocument = XmlManager.Instance.Load("armor.xml");
				XmlNode objArmorNode = objXmlArmorDocument.SelectSingleNode("/chummer/armors/armor[id = \"" + ExternalId + "\"]");
				if (objArmorNode != null)
				{
					if (objArmorNode["translate"] != null)
						_strAltName = objArmorNode["translate"].InnerText;
					if (objArmorNode["altpage"] != null)
						_strAltPage = objArmorNode["altpage"].InnerText;
				}

				objArmorNode = objXmlArmorDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objArmorNode != null)
				{
					if (objArmorNode.Attributes["translate"] != null)
						_strAltCategory = objArmorNode.Attributes["translate"].InnerText;
				}
			}

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
				_strLocation = string.Empty;
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("armor");
			objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("name_english", _strName);
			objWriter.WriteElementString("category", DisplayCategory);
			objWriter.WriteElementString("category_english", _strCategory);
			objWriter.WriteElementString("armorvalue", TotalArmorValue.ToString());
			objWriter.WriteElementString("avail", TotalAvail);
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteStartElement("armormods");
			foreach (ArmorMod objMod in ArmorMods)
			{
				objMod.Print(objWriter);
			}
			objWriter.WriteEndElement();
			objWriter.WriteStartElement("gears");
			foreach (Gear objGear in Gears)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink = (Commlink)objGear;
					objCommlink.Print(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Print(objWriter);
				}
				else
				{
					objGear.Print(objWriter);
				}
			}
			objWriter.WriteEndElement();
			objWriter.WriteElementString("extra", LanguageManager.Instance.TranslateExtra(_strExtra));
			objWriter.WriteElementString("location", _strLocation);
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Armor's value.
		/// </summary>
		public string ArmorValue
		{
			get
			{
				return _strArmorValue;
			}
			set
			{
				_strArmorValue = value;
			}
		}

		/// <summary>
		/// Damage done to the Armor's Rating.
		/// </summary>
		public int Damage
		{
			get
			{
				return _intDamage;
			}
			set
			{
				_intDamage = value;

				int intTotalArmor = Convert.ToInt32(_strArmorValue);

				// Go through all of the Mods for this piece of Armor and add the Armor value.
				foreach (ArmorMod objMod in ArmorMods)
				{
					if (objMod.Equipped)
						intTotalArmor += objMod.ArmorValue;
				}

				if (_intDamage < 0)
					_intDamage = 0;
				if (_intDamage > intTotalArmor)
					_intDamage = intTotalArmor;
			}
		}

		/// <summary>
		/// Armor's Capacity.
		/// </summary>
		public string ArmorCapacity
		{
			get
			{
				return _strArmorCapacity;
			}
			set
			{
				_strArmorCapacity = value;
			}
		}

		/// <summary>
		/// Armor's Cost.
		/// </summary>
		public int Cost
		{
			get
			{
				return Convert.ToInt32(_strCost);
			}
			set
			{
				_strCost = value.ToString();
			}
		}

		/// <summary>
		/// The Armor's total value including Modifications.
		/// </summary>
		public int TotalArmorValue
		{
			get
			{
				int intTotalArmor = Convert.ToInt32(_strArmorValue);
				
				// Go through all of the Mods for this piece of Armor and add the Armor value.
				foreach (ArmorMod objMod in ArmorMods)
				{
					if (objMod.Equipped)
						intTotalArmor += objMod.ArmorValue;
				}

				intTotalArmor -= _intDamage;

				return intTotalArmor;
			}
		}

		/// <summary>
		/// The Armor's total Cost including Modifications.
		/// </summary>
		public int TotalCost
		{
			get
			{
				int intTotalCost = Convert.ToInt32(_strCost);

				if (DiscountCost)
					intTotalCost = Convert.ToInt32(Convert.ToDouble(intTotalCost, GlobalOptions.Instance.CultureInfo) * 0.9);

				// Go through all of the Mods for this piece of Armor and add the Cost value.
				foreach (ArmorMod objMod in ArmorMods)
					intTotalCost += objMod.TotalCost;

				// Go through all of the Gear for this piece of Armor and add the Cost value.
				foreach (Gear objGear in Gears)
					intTotalCost += objGear.TotalCost;

				return intTotalCost;
			}
		}

		/// <summary>
		/// Cost for just the Armor.
		/// </summary>
		public int OwnCost
		{
			get
			{
				int intTotalCost = Convert.ToInt32(_strCost);

				if (DiscountCost)
					intTotalCost = Convert.ToInt32(Convert.ToDouble(intTotalCost, GlobalOptions.Instance.CultureInfo) * 0.9);

				return intTotalCost;
			}
		}

		/// <summary>
		/// Location.
		/// </summary>
		public string Location
		{
			get
			{
				return _strLocation;
			}
			set
			{
				_strLocation = value;
			}
		}
		#endregion

		#region Complex Properties
		/// <summary>
		/// Calculated Capacity of the Armor.
		/// </summary>
		public string CalculatedCapacity
		{
			get
			{
				string strReturn = "";

				// If an Armor Capacity is specified for the Armor, use that value. Otherwise, use the Armor's Rating.
				if (_strArmorCapacity == "" || _strArmorCapacity == "0")
				{
                    strReturn = _strArmorValue;
				}
				else
					strReturn = _strArmorCapacity;

				return strReturn;
			}
		}

		/// <summary>
		/// The amount of Capacity remaining in the Gear.
		/// </summary>
		public int CapacityRemaining
		{
			get
			{
				int intCapacity = 0;
				// Get the Armor base Capacity.
				intCapacity = Convert.ToInt32(CalculatedCapacity);

				// If there is no Capacity (meaning that the Armor Suit Capacity or Maximum Armor Modification rule is turned off depending on the type of Armor), don't bother to calculate the remaining
				// Capacity since it's disabled and return 0 instead.
				if (intCapacity == 0)
					return 0;

				// Calculate the remaining Capacity for a Suit of Armor.
				if (_strArmorCapacity != "0" && _strArmorCapacity != "" && _objCharacter.Options.ArmorSuitCapacity)
				{
					// Run through its Armor Mods and deduct the Capacity costs.
					foreach (ArmorMod objMod in ArmorMods)
					{
						string strCapacity = objMod.CalculatedCapacity;
						if (strCapacity.Contains("/["))
						{
							// If this is a multiple-capacity item, use only the second half.
							int intPos = strCapacity.IndexOf("/[");
							strCapacity = strCapacity.Substring(intPos + 1);
						}

						if (strCapacity.Contains("["))
							strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
						if (strCapacity == "*")
							strCapacity = "0";
						intCapacity -= Convert.ToInt32(strCapacity);
					}

					// Run through its Gear and deduct the Armor Capacity costs.
					foreach (Gear objGear in Gears)
					{
						string strCapacity = objGear.CalculatedArmorCapacity;
						if (strCapacity.Contains("/["))
						{
							// If this is a multiple-capacity item, use only the second half.
							int intPos = strCapacity.IndexOf("/[");
							strCapacity = strCapacity.Substring(intPos + 1);
						}

						if (strCapacity.Contains("["))
							strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
						if (strCapacity == "*")
							strCapacity = "0";
						intCapacity -= Convert.ToInt32(strCapacity);
					}
				}

				// Calculate the remaining Capacity for a standard piece of Armor using the Maximum Armor Modifications rules.
				if ((_strArmorCapacity == "0" || _strArmorCapacity == "") && _objCharacter.Options.MaximumArmorModifications)
				{
					// Run through its Armor Mods and deduct the Rating (or 1 if it has no Rating).
					foreach (ArmorMod objMod in ArmorMods)
					{
						if (objMod.Rating > 0)
							intCapacity -= objMod.Rating;
						else
							intCapacity -= 1;
					}

					// Run through its Gear and deduct the Rating (or 1 if it has no Rating).
					foreach (Gear objGear in Gears)
					{
						if (objGear.Rating > 0)
							intCapacity -= objGear.Rating;
						else
							intCapacity -= 1;
					}
				}

				return intCapacity;
			}
		}

		/// <summary>
		/// Capacity display style;
		/// </summary>
		public CapacityStyle CapacityDisplayStyle
		{
			get
			{
				CapacityStyle objReturn = CapacityStyle.Zero;
				
				if ((_strArmorCapacity == "" || _strArmorCapacity == "0") && _objCharacter.Options.MaximumArmorModifications)
					objReturn = CapacityStyle.PerRating;
				if (_strArmorCapacity != "" && _strArmorCapacity != "0" && _objCharacter.Options.ArmorSuitCapacity)
					objReturn = CapacityStyle.Standard;

				return objReturn;
			}
		}
		#endregion
	}

	/// <summary>
	/// Grade of Cyberware or Bioware.
	/// </summary>
	public class Grade
	{
		private string _strName = "Standard";
		private string _strAltName = "";
		private decimal _decEss = 1.0m;
		private double _dblCost = 1.0;
		private int _intAvail = 0;
		private string _strSource = "SR5";

		#region Constructor and Load Methods
		public Grade()
		{
		}

		/// <summary>
		/// Load the Grade from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public void Load(XmlNode objNode)
		{
			_strName = objNode["name"].InnerText;
			if (objNode["translate"] != null)
				_strAltName = objNode["translate"].InnerText;
			_decEss = Convert.ToDecimal(objNode["ess"].InnerText, GlobalOptions.Instance.CultureInfo);
			_dblCost = Convert.ToDouble(objNode["cost"].InnerText, GlobalOptions.Instance.CultureInfo);
			_intAvail = Convert.ToInt32(objNode["avail"].InnerText, GlobalOptions.Instance.CultureInfo);
			_strSource = objNode["source"].InnerText;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The English name of the Grade.
		/// </summary>
		public string Name
		{
			get
			{
				return _strName;
			}
		}

		/// <summary>
		/// The name of the Grade as it should be displayed in lists.
		/// </summary>
		public string DisplayName
		{
			get
			{
				if (_strAltName != string.Empty)
					return _strAltName;
				else
					return _strName;
			}
		}

		/// <summary>
		/// The Grade's Essence cost multiplier.
		/// </summary>
		public decimal Essence
		{
			get
			{
				return _decEss;
			}
		}

		/// <summary>
		/// The Grade's cost multiplier.
		/// </summary>
		public double Cost
		{
			get
			{
				return _dblCost;
			}
		}

		/// <summary>
		/// The Grade's Availability modifier.
		/// </summary>
		public int Avail
		{
			get
			{
				return _intAvail;
			}
		}

		/// <summary>
		/// Sourcebook.
		/// </summary>
		public string Source
		{
			get
			{
				return _strSource;
			}
		}

		/// <summary>
		/// Whether or not the Grade is for Adapsin.
		/// </summary>
		public bool Adapsin
		{
			get
			{
				return _strName.Contains("(Adapsin)");
			}
		}

		/// <summary>
		/// Whether or not this is a Second-Hand Grade.
		/// </summary>
		public bool SecondHand
		{
			get
			{
				return _strName.Contains("(Second-Hand)");
			}
		}
		#endregion
	}

	/// <summary>
	/// List of Grades for either Cyberware or Bioware.
	/// </summary>
	public class GradeList : IEnumerable<Grade>
	{
		private List<Grade> _lstGrades = new List<Grade>();

		#region Methods
		/// <summary>
		/// Fill the list of CyberwareGrades from the XML files.
		/// </summary>
		/// <param name="objSource">Source to load the Grades from, either Bioware or Cyberware.</param>
		public void LoadList(Improvement.ImprovementSource objSource)
		{
			string strXmlFile = "";
			if (objSource == Improvement.ImprovementSource.Bioware)
				strXmlFile = "bioware.xml";
			else
				strXmlFile = "cyberware.xml";
			XmlDocument objXMlDocument = XmlManager.Instance.Load(strXmlFile);
			
			foreach (XmlNode objNode in objXMlDocument.SelectNodes("/chummer/grades/grade"))
			{
				Grade objGrade = new Grade();
				objGrade.Load(objNode);
				_lstGrades.Add(objGrade);
			}
		}

		/// <summary>
		/// Retrieve the Standard Grade from the list.
		/// </summary>
		public Grade GetGrade(string strGrade)
		{
			Grade objReturn = new Grade();
			foreach (Grade objGrade in _lstGrades)
			{
				if (objGrade.Name == "Standard")
				{
					objReturn = objGrade;
					break;
				}
			}

			if (strGrade != "Standard")
			{
				foreach (Grade objGrade in _lstGrades)
				{
					if (objGrade.Name == strGrade)
					{
						objReturn = objGrade;
						break;
					}
				}
			}

			return objReturn;
		}
		#endregion

		#region Enumeration Methods
		public IEnumerator<Grade> GetEnumerator()
		{
			return this._lstGrades.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}

	/// <summary>
	/// A piece of Cyberware.
	/// </summary>
	public class Cyberware : Equipment
	{
		private string _strLimbSlot = "";
		private string _strESS = "";
		private string _strCapacity = "";
		private string _strSubsystems = "";
		private bool _blnSuite = false;
		private string _strLocation = "";
		private Guid _guiWeaponID = new Guid();
		private Grade _objGrade = new Grade();
		private XmlNode _nodAllowGear;
		private Improvement.ImprovementSource _objImprovementSource = Improvement.ImprovementSource.Cyberware;
		private int _intEssenceDiscount = 0;
		private string _strForceGrade = "";

		#region Helper Methods
		/// <summary>
		/// Convert a string to a Grade.
		/// </summary>
		/// <param name="strValue">String value to convert.</param>
		public Grade ConvertToCyberwareGrade(string strValue, Improvement.ImprovementSource objSource)
		{
			if (objSource == Improvement.ImprovementSource.Bioware)
			{
				foreach (Grade objGrade in GlobalOptions.BiowareGrades)
				{
					if (objGrade.Name == strValue)
						return objGrade;
				}

				return GlobalOptions.BiowareGrades.GetGrade("Standard");
			}
			else
			{
				foreach (Grade objGrade in GlobalOptions.CyberwareGrades)
				{
					if (objGrade.Name == strValue)
						return objGrade;
				}

				return GlobalOptions.CyberwareGrades.GetGrade("Standard");
			}
		}
		#endregion

		#region Constructor, Create, Save, Load, and Print Methods
		public Cyberware(Character objCharacter) : base(objCharacter)
		{
			// Create the GUID for the new piece of Cyberware.
			_guiID = Guid.NewGuid();
		}

		/// Create a Cyberware from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlCyberware">XmlNode to create the object from.</param>
		/// <param name="objCharacter">Character object the Cyberware will be added to.</param>
		/// <param name="objGrade">Grade of the selected piece.</param>
		/// <param name="objSource">Source of the piece.</param>
		/// <param name="intRating">Selected Rating of the piece of Cyberware.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		/// <param name="objWeapons">List of Weapons that should be added to the Character.</param>
		/// <param name="objWeaponNodes">List of TreeNode to represent the Weapons added.</param>
		/// <param name="blnCreateImprovements">Whether or not Improvements should be created.</param>
		/// <param name="blnCreateChildren">Whether or not child items should be created.</param>
		/// <param name="strForced">Force a particular value to be selected by an Improvement prompts.</param>
		public void Create(XmlNode objXmlCyberware, Character objCharacter, Grade objGrade, Improvement.ImprovementSource objSource, int intRating, TreeNode objNode, List<Weapon> objWeapons, List<TreeNode> objWeaponNodes, bool blnCreateImprovements = true, bool blnCreateChildren = true, string strForced = "")
		{
            _guidExternalID = Guid.Parse(objXmlCyberware["id"].InnerText);
			_strName = objXmlCyberware["name"].InnerText;
			_strCategory = objXmlCyberware["category"].InnerText;
			if (objXmlCyberware["limbslot"] != null)
				_strLimbSlot = objXmlCyberware["limbslot"].InnerText;
			_objGrade = objGrade;
			_intRating = intRating;
			_strESS = objXmlCyberware["ess"].InnerText;
			_strCapacity = objXmlCyberware["capacity"].InnerText;
			_strAvail = objXmlCyberware["avail"].InnerText;
			_strSource = objXmlCyberware["source"].InnerText;
			_strPage = objXmlCyberware["page"].InnerText;
			_nodBonus = objXmlCyberware["bonus"];
            _nodConditional = objXmlCyberware["conditional"];
			_nodAllowGear = objXmlCyberware["allowgear"];
			_objImprovementSource = objSource;
			try
			{
				_intMaxRating = Convert.ToInt32(objXmlCyberware["rating"].InnerText);
			}
			catch
			{
				_intMaxRating = 0;
			}
			try
			{
				_intMinRating = Convert.ToInt32(objXmlCyberware["minrating"].InnerText);
			}
			catch
			{
				if (_intMaxRating > 0)
					_intMinRating = 1;
				else
					_intMinRating = 0;
			}
			if (objXmlCyberware["forcegrade"] != null)
				_strForceGrade = objXmlCyberware["forcegrade"].InnerText;

			if (GlobalOptions.Instance.Language != "en-us")
			{
				string strXmlFile = "";
				string strXPath = "";
				if (_objImprovementSource == Improvement.ImprovementSource.Bioware)
				{
					strXmlFile = "bioware.xml";
					strXPath = "/chummer/biowares/bioware";
				}
				else
				{
					strXmlFile = "cyberware.xml";
					strXPath = "/chummer/cyberwares/cyberware";
				}
				XmlDocument objXmlDocument = XmlManager.Instance.Load(strXmlFile);
				XmlNode objCyberwareNode = objXmlDocument.SelectSingleNode(strXPath + "[id = \"" + ExternalId + "\"]");
				if (objCyberwareNode != null)
				{
					if (objCyberwareNode["translate"] != null)
						_strAltName = objCyberwareNode["translate"].InnerText;
					if (objCyberwareNode["altpage"] != null)
						_strAltPage = objCyberwareNode["altpage"].InnerText;
				}

				objCyberwareNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objCyberwareNode != null)
				{
					if (objCyberwareNode.Attributes["translate"] != null)
						_strAltCategory = objCyberwareNode.Attributes["translate"].InnerText;
				}
			}

			// Add Subsytem information if applicable.
			if (objXmlCyberware["subsystems"] != null)
			{
				string strSubsystem = "";
				foreach (XmlNode objXmlSubsystem in objXmlCyberware.SelectNodes("subsystems/subsystem"))
				{
					strSubsystem += objXmlSubsystem.InnerText + ",";
				}
				_strSubsystems = strSubsystem;
			}

			// Check for a Variable Cost.
            if (objXmlCyberware["cost"].InnerText.StartsWith("Variable"))
                _strCost = VariableCost(objXmlCyberware["cost"].InnerText, DisplayNameShort).ToString();
            else
                _strCost = objXmlCyberware["cost"].InnerText;

			// Add Cyberweapons if applicable.
			if (objXmlCyberware["addweapon"] != null)
			{
				XmlDocument objXmlWeaponDocument = XmlManager.Instance.Load("weapons.xml");

				// More than one Weapon can be added, so loop through all occurrences.
				foreach (XmlNode objXmlAddWeapon in objXmlCyberware.SelectNodes("addweapon"))
				{
					XmlNode objXmlWeapon = objXmlWeaponDocument.SelectSingleNode("/chummer/weapons/weapon[id = \"" + objXmlAddWeapon.InnerText + "\"]");

					TreeNode objGearWeaponNode = new TreeNode();
					Weapon objGearWeapon = new Weapon(objCharacter);
					objGearWeapon.Create(objXmlWeapon, objCharacter, objGearWeaponNode, null, null, null);
					objGearWeaponNode.ForeColor = SystemColors.GrayText;
					objWeaponNodes.Add(objGearWeaponNode);
					objWeapons.Add(objGearWeapon);

					_guiWeaponID = Guid.Parse(objGearWeapon.InternalId);
				}
			}

			// If the piece grants a bonus, pass the information to the Improvement Manager.
			if (blnCreateImprovements)
			{
				ImprovementManager objImprovementManager = new ImprovementManager(objCharacter);
				if (strForced != "")
					objImprovementManager.ForcedValue = strForced;

				if (!objImprovementManager.CreateImprovements(objSource, _guiID.ToString(), _nodBonus, false, _intRating, DisplayNameShort))
				{
					_guiID = Guid.Empty;
					return;
				}
				if (objImprovementManager.SelectedValue != "")
					_strLocation = objImprovementManager.SelectedValue;

                // Conditional Improvements.
                if (!objImprovementManager.CreateImprovements(objSource, _guiID.ToString(), _nodConditional, false, 1, DisplayNameShort, true))
                {
                    _guiID = Guid.Empty;
                    return;
                }
			}

			// Create the TreeNode for the new item.
			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();

			// If we've just added a new base item, see if there are any subsystems that should automatically be added.
			if (objXmlCyberware["subsystems"] != null && blnCreateChildren)
			{
				XmlDocument objXmlDocument = new XmlDocument();
				if (objSource == Improvement.ImprovementSource.Bioware)
					objXmlDocument = XmlManager.Instance.Load("bioware.xml");
				else
					objXmlDocument = XmlManager.Instance.Load("cyberware.xml");

				XmlNodeList objXmlSubsystemList;
				if (objSource == Improvement.ImprovementSource.Bioware)
					objXmlSubsystemList = objXmlDocument.SelectNodes("/chummer/biowares/bioware[capacity = \"[*]\" and contains(\"" + _strSubsystems + "\", category)]");
				else
					objXmlSubsystemList = objXmlDocument.SelectNodes("/chummer/cyberwares/cyberware[capacity = \"[*]\" and contains(\"" + _strSubsystems + "\", category)]");
				foreach (XmlNode objXmlSubsystem in objXmlSubsystemList)
				{
					Cyberware objSubsystem = new Cyberware(objCharacter);
					objSubsystem.Name = objXmlSubsystem["name"].InnerText;
					objSubsystem.Category = objXmlSubsystem["category"].InnerText;
					objSubsystem.Grade = objGrade;
					objSubsystem.Rating = 0;
					objSubsystem.ESS = objXmlSubsystem["ess"].InnerText;
					objSubsystem.Capacity = objXmlSubsystem["capacity"].InnerText;
					objSubsystem.Avail = objXmlSubsystem["avail"].InnerText;
					objSubsystem.Cost = objXmlSubsystem["cost"].InnerText;
					objSubsystem.Source = objXmlSubsystem["source"].InnerText;
					objSubsystem.Page = objXmlSubsystem["page"].InnerText;
					objSubsystem.Bonus = objXmlSubsystem["bonus"];
					try
					{
						objSubsystem.MaxRating = Convert.ToInt32(objXmlCyberware["rating"].InnerText);
					}
					catch
					{
						objSubsystem.MaxRating = 0;
					}

					// If there are any bonuses, create the Improvements for them.
					if (objXmlSubsystem["bonus"] != null && blnCreateImprovements)
					{
						ImprovementManager objImprovementManager = new ImprovementManager(objCharacter);
						if (!objImprovementManager.CreateImprovements(objSource, objSubsystem.InternalId, objXmlSubsystem.SelectSingleNode("bonus"), false, 1, objSubsystem.DisplayNameShort))
						{
							objSubsystem._guiID = Guid.Empty;
							return;
						}
					}

					objSubsystem.Parent = this;

					Cyberwares.Add(objSubsystem);

					TreeNode objSubsystemNode = new TreeNode();
					objSubsystemNode.Text = objSubsystem.DisplayName;
					objSubsystemNode.Tag = objSubsystem.InternalId;
					objSubsystemNode.ForeColor = SystemColors.GrayText;
					objNode.Nodes.Add(objSubsystemNode);
					objNode.Expand();
				}
			}
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("cyberware");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("category", _strCategory);
			objWriter.WriteElementString("limbslot", _strLimbSlot);
			objWriter.WriteElementString("ess", _strESS);
			objWriter.WriteElementString("capacity", _strCapacity);
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("minrating", _intMinRating.ToString());
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("subsystems", _strSubsystems);
			objWriter.WriteElementString("grade", _objGrade.Name);
			objWriter.WriteElementString("location", _strLocation);
			objWriter.WriteElementString("suite", _blnSuite.ToString());
			objWriter.WriteElementString("essdiscount", _intEssenceDiscount.ToString());
			objWriter.WriteElementString("forcegrade", _strForceGrade);
			if (_nodBonus != null)
				objWriter.WriteRaw(_nodBonus.OuterXml);
			else
				objWriter.WriteElementString("bonus", "");
            if (_nodConditional != null)
                objWriter.WriteRaw(_nodConditional.OuterXml);
            else
                objWriter.WriteElementString("conditional", "");
			if (_nodAllowGear != null)
				objWriter.WriteRaw(_nodAllowGear.OuterXml);
			objWriter.WriteElementString("improvementsource", _objImprovementSource.ToString());
			if (_guiWeaponID != Guid.Empty)
				objWriter.WriteElementString("weaponguid", _guiWeaponID.ToString());
			objWriter.WriteStartElement("children");
			foreach (Cyberware objChild in Cyberwares)
			{
				objChild.Save(objWriter);
			}
			objWriter.WriteEndElement();
			if (Gears.Count > 0)
			{
				objWriter.WriteStartElement("gears");
				foreach (Gear objGear in Gears)
				{
					// Use the Gear's SubClass if applicable.
					if (objGear.GetType() == typeof(Commlink))
					{
						Commlink objCommlink = new Commlink(_objCharacter);
						objCommlink = (Commlink)objGear;
						objCommlink.Save(objWriter);
					}
					else if (objGear.GetType() == typeof(OperatingSystem))
					{
						OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
						objOperatinSystem = (OperatingSystem)objGear;
						objOperatinSystem.Save(objWriter);
					}
					else
					{
						objGear.Save(objWriter);
					}
				}
				objWriter.WriteEndElement();
			}
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Attribute from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public override void Load(XmlNode objNode, bool blnCopy = false)
		{
			Improvement objImprovement = new Improvement();
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_strCategory = objNode["category"].InnerText;
			_strLimbSlot = objNode["limbslot"].InnerText;
			_strESS = objNode["ess"].InnerText;
			_strCapacity = objNode["capacity"].InnerText;
			_strAvail = objNode["avail"].InnerText;
			_strCost = objNode["cost"].InnerText;
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;
			_intRating = Convert.ToInt32(objNode["rating"].InnerText);
			_intMinRating = Convert.ToInt32(objNode["minrating"].InnerText);
			_intMaxRating = Convert.ToInt32(objNode["maxrating"].InnerText);
			_strSubsystems = objNode["subsystems"].InnerText;
			_objGrade = ConvertToCyberwareGrade(objNode["grade"].InnerText, _objImprovementSource);
			_strLocation = objNode["location"].InnerText;
			_blnSuite = Convert.ToBoolean(objNode["suite"].InnerText);
			_intEssenceDiscount = Convert.ToInt32(objNode["essdiscount"].InnerText);
			_strForceGrade = objNode["forcegrade"].InnerText;
			_nodBonus = objNode["bonus"];
            _nodConditional = objNode["conditional"];
			if (objNode["allowgear"] != null)
				_nodAllowGear = objNode["allowgear"];
			_objImprovementSource = objImprovement.ConvertToImprovementSource(objNode["improvementsource"].InnerText);
            if (objNode["weaponguid"] != null)
				_guiWeaponID = Guid.Parse(objNode["weaponguid"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				string strXmlFile = "";
				string strXPath = "";
				if (_objImprovementSource == Improvement.ImprovementSource.Bioware)
				{
					strXmlFile = "bioware.xml";
					strXPath = "/chummer/biowares/bioware";
				}
				else
				{
					strXmlFile = "cyberware.xml";
					strXPath = "/chummer/cyberwares/cyberware";
				}
				XmlDocument objXmlDocument = XmlManager.Instance.Load(strXmlFile);
				XmlNode objCyberwareNode = objXmlDocument.SelectSingleNode(strXPath + "[id = \"" + ExternalId + "\"]");
				if (objCyberwareNode != null)
				{
					if (objCyberwareNode["translate"] != null)
						_strAltName = objCyberwareNode["translate"].InnerText;
					if (objCyberwareNode["altpage"] != null)
						_strAltPage = objCyberwareNode["altpage"].InnerText;
				}

				objCyberwareNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objCyberwareNode != null)
				{
					if (objCyberwareNode.Attributes["translate"] != null)
						_strAltCategory = objCyberwareNode.Attributes["translate"].InnerText;
				}
			}

			if (objNode["cyberware"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("children/cyberware");
				foreach (XmlNode nodChild in nodChildren)
				{
					Cyberware objChild = new Cyberware(_objCharacter);
					objChild.Load(nodChild, blnCopy);
					objChild.Parent = this;
					Cyberwares.Add(objChild);
				}
			}

			if (objNode["gears"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("gears/gear");
				foreach (XmlNode nodChild in nodChildren)
				{
					switch (nodChild["category"].InnerText)
					{
						case "Commlink":
						case "Commlink Upgrade":
							Commlink objCommlink = new Commlink(_objCharacter);
							objCommlink.Load(nodChild, blnCopy);
							objCommlink.Parent = this;
							Gears.Add(objCommlink);
							break;
						case "Commlink Operating System":
						case "Commlink Operating System Upgrade":
							OperatingSystem objOperatingSystem = new OperatingSystem(_objCharacter);
							objOperatingSystem.Load(nodChild, blnCopy);
							objOperatingSystem.Parent = this;
							Gears.Add(objOperatingSystem);
							break;
						default:
							Gear objGear = new Gear(_objCharacter);
							objGear.Load(nodChild, blnCopy);
							objGear.Parent = this;
							Gears.Add(objGear);
							break;
					}
				}
			}

			_strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("cyberware");
			if (_strLimbSlot == "")
				objWriter.WriteElementString("name", DisplayNameShort);
			else
				objWriter.WriteElementString("name", DisplayNameShort + " (" + LanguageManager.Instance.GetString("String_AttributeBODShort") + " " + TotalBody.ToString() + ", " + LanguageManager.Instance.GetString("String_AttributeAGIShort") + " " + TotalAgility.ToString() + ", " + LanguageManager.Instance.GetString("String_AttributeSTRShort") + " " + TotalStrength.ToString() + ")");
			objWriter.WriteElementString("category", DisplayCategory);
			objWriter.WriteElementString("ess", CalculatedESS.ToString());
			objWriter.WriteElementString("capacity", _strCapacity);
			objWriter.WriteElementString("avail", TotalAvail);
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			try
			{
				objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			}
			catch
			{
				objWriter.WriteElementString("source", _strSource);
			}
			objWriter.WriteElementString("page", Page);
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("minrating", _intMinRating.ToString());
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("subsystems", _strSubsystems);
			objWriter.WriteElementString("grade", _objGrade.DisplayName);
			objWriter.WriteElementString("location", _strLocation);
			objWriter.WriteElementString("improvementsource", _objImprovementSource.ToString());
			if (Gears.Count > 0)
			{
				objWriter.WriteStartElement("gears");
				foreach (Gear objGear in Gears)
				{
					// Use the Gear's SubClass if applicable.
					if (objGear.GetType() == typeof(Commlink))
					{
						Commlink objCommlink = new Commlink(_objCharacter);
						objCommlink = (Commlink)objGear;
						objCommlink.Print(objWriter);
					}
					else if (objGear.GetType() == typeof(OperatingSystem))
					{
						OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
						objOperatinSystem = (OperatingSystem)objGear;
						objOperatinSystem.Print(objWriter);
					}
					else
					{
						objGear.Print(objWriter);
					}
				}
				objWriter.WriteEndElement();
			}
			objWriter.WriteStartElement("children");
			foreach (Cyberware objChild in Cyberwares)
			{
				objChild.Print(objWriter);
			}
			objWriter.WriteEndElement();
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Guid of a Cyberware Weapon.
		/// </summary>
		public string WeaponID
		{
			get
			{
				return _guiWeaponID.ToString();
			}
			set
			{
				_guiWeaponID = Guid.Parse(value);
			}
		}

		/// <summary>
		/// AllowGear node from the XML file.
		/// </summary>
		public XmlNode AllowGear
		{
			get
			{
				return _nodAllowGear;
			}
			set
			{
				_nodAllowGear = value;
			}
		}

		/// <summary>
		/// ImprovementSource Type.
		/// </summary>
		public Improvement.ImprovementSource SourceType
		{
			get
			{
				return _objImprovementSource;
			}
			set
			{
				_objImprovementSource = value;
			}
		}

		/// <summary>
		/// The name of the object as it should be displayed in lists. Qty Name (Rating) (Extra).
		/// </summary>
		public new string DisplayName
		{
			get
			{
				string strReturn = DisplayNameShort;

				if (_intRating > 0)
					strReturn += " (" + LanguageManager.Instance.GetString("String_Rating") + " " + _intRating.ToString() + ")";
				if (_strLocation != "")
				{
					LanguageManager.Instance.Load(this);
					// Attempt to retrieve the Attribute name.
					try
					{
						if (LanguageManager.Instance.GetString("String_Attribute" + _strLocation + "Short") != "")
							strReturn += " (" + LanguageManager.Instance.GetString("String_Attribute" + _strLocation + "Short") + ")";
						else
							strReturn += " (" + LanguageManager.Instance.TranslateExtra(_strLocation) + ")";
					}
					catch
					{
						strReturn += " (" + LanguageManager.Instance.TranslateExtra(_strLocation) + ")";
					}
				}
				return strReturn;
			}
		}

		/// <summary>
		/// The body "slot" a Cyberlimb occupies.
		/// </summary>
		public string LimbSlot
		{
			get
			{
				return _strLimbSlot;
			}
			set
			{
				_strLimbSlot = value;
			}
		}

		/// <summary>
		/// The location of a Cyberlimb.
		/// </summary>
		public string Location
		{
			get
			{
				return _strLocation;
			}
			set
			{
				_strLocation = value;
			}
		}

		/// <summary>
		/// Essence cost of the Cyberware.
		/// </summary>
		public string ESS
		{
			get
			{
				return _strESS;
			}
			set
			{
				_strESS = value;
			}
		}

		/// <summary>
		/// Cyberware capacity.
		/// </summary>
		public string Capacity
		{
			get
			{
				return _strCapacity;
			}
			set
			{
				_strCapacity = value;
			}
		}

		/// <summary>
		/// Cost.
		/// </summary>
		public string Cost
		{
			get
			{
				return _strCost;
			}
			set
			{
				_strCost = value;
			}
		}

		/// <summary>
		/// Grade level of the Cyberware.
		/// </summary>
		public Grade Grade
		{
			get
			{
				return _objGrade;
			}
			set
			{
				_objGrade = value;
			}
		}

		/// <summary>
		/// The Categories of allowable Subsystems.
		/// </summary>
		public string Subsytems
		{
			get
			{
				return _strSubsystems;
			}
			set
			{
				_strSubsystems = value;
			}
		}

		/// <summary>
		/// Whether or not the piece of Cyberware is part of a Cyberware Suite.
		/// </summary>
		public bool Suite
		{
			get
			{
				return _blnSuite;
			}
			set
			{
				_blnSuite = value;
			}
		}

		/// <summary>
		/// Essence cost discount.
		/// </summary>
		public int ESSDiscount
		{
			get
			{
				return _intEssenceDiscount;
			}
			set
			{
				_intEssenceDiscount = value;
			}
		}

		/// <summary>
		/// Grade that the Cyberware should be forced to use, if applicable.
		/// </summary>
		public string ForceGrade
		{
			get
			{
				return _strForceGrade;
			}
		}
		#endregion

		#region Complex Properties
		/// <summary>
		/// Total Availablility of the Cyberware and its plugins.
		/// </summary>
		public new string TotalAvail
		{
			get
			{
				// If the Avail starts with "+", return the base string and don't try to calculate anything since we're looking at a child component.
				if (_strAvail.StartsWith("+"))
				{
					if (_strAvail.Contains("Rating"))
					{
						// If the availability is determined by the Rating, evaluate the expression.
						XmlDocument objXmlDocument = new XmlDocument();
						XPathNavigator nav = objXmlDocument.CreateNavigator();

						string strAvail = "";
						string strAvailExpr = _strAvail.Substring(1, _strAvail.Length - 1);

						if (strAvailExpr.Substring(strAvailExpr.Length - 1, 1) == "F" || strAvailExpr.Substring(strAvailExpr.Length - 1, 1) == "R")
						{
							strAvail = strAvailExpr.Substring(strAvailExpr.Length - 1, 1);
							// Remove the trailing character if it is "F" or "R".
							strAvailExpr = strAvailExpr.Substring(0, strAvailExpr.Length - 1);
						}
						XPathExpression xprAvail = nav.Compile(strAvailExpr.Replace("Rating", _intRating.ToString()));
						return "+" + nav.Evaluate(xprAvail) + strAvail;
					}
					else
						return _strAvail;
				}

				string strCalculated = "";
				string strReturn = "";

				// Second Hand Cyberware has a reduced Availability.
				int intAvailModifier = 0;

				// Apply the Grade's Avail modifier.
				intAvailModifier = Grade.Avail;

				if (_strAvail.Contains("Rating"))
				{
					// If the availability is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strAvail = "";
					string strAvailExpr = _strAvail;

					if (strAvailExpr.Substring(strAvailExpr.Length - 1, 1) == "F" || strAvailExpr.Substring(strAvailExpr.Length - 1, 1) == "R")
					{
						strAvail = strAvailExpr.Substring(strAvailExpr.Length - 1, 1);
						// Remove the trailing character if it is "F" or "R".
						strAvailExpr = strAvailExpr.Substring(0, strAvailExpr.Length - 1);
					}
					XPathExpression xprAvail = nav.Compile(strAvailExpr.Replace("Rating", _intRating.ToString()));
					strCalculated = (Convert.ToInt32(nav.Evaluate(xprAvail)) + intAvailModifier).ToString() + strAvail;
				}
				else
				{
					if (_strAvail.StartsWith("FixedValues"))
                        strCalculated = FixedValue(_strAvail);
					else
					{
						// Just a straight cost, so return the value.
						string strAvail = "";
						if (_strAvail.Contains("F") || _strAvail.Contains("R"))
						{
							strAvail = _strAvail.Substring(_strAvail.Length - 1, 1);
							strCalculated = (Convert.ToInt32(_strAvail.Substring(0, _strAvail.Length - 1)) + intAvailModifier) + strAvail;
						}
						else
							strCalculated = (Convert.ToInt32(_strAvail) + intAvailModifier).ToString();
					}
				}

				int intAvail = 0;
				string strAvailText = "";
				if (strCalculated.Contains("F") || strCalculated.Contains("R"))
				{
					strAvailText = strCalculated.Substring(strCalculated.Length - 1);
					intAvail = Convert.ToInt32(strCalculated.Replace(strAvailText, string.Empty));
				}
				else
					intAvail = Convert.ToInt32(strCalculated);

				// Run through the child items and increase the Avail by any Mod whose Avail contains "+".
				foreach (Cyberware objChild in Cyberwares)
				{
					if (objChild.Avail.Contains("+"))
					{
						string strChildAvail = objChild.Avail;
						if (objChild.Avail.Contains("Rating"))
						{
							strChildAvail = strChildAvail.Replace("Rating", objChild.Rating.ToString());
							string strChildAvailText = "";
							if (strChildAvail.Contains("R") || strChildAvail.Contains("F"))
							{
								strChildAvailText = strChildAvail.Substring(objChild.Avail.Length - 1);
								strChildAvail = strChildAvail.Replace(strChildAvailText, string.Empty);
							}

							// If the availability is determined by the Rating, evaluate the expression.
							XmlDocument objXmlDocument = new XmlDocument();
							XPathNavigator nav = objXmlDocument.CreateNavigator();

							string strChildAvailExpr = strChildAvail;

							// Remove the "+" since the expression can't be evaluated if it starts with this.
							XPathExpression xprAvail = nav.Compile(strChildAvailExpr.Replace("+", ""));
							strChildAvail = "+" + nav.Evaluate(xprAvail);
							if (strChildAvailText != "")
								strChildAvail += strChildAvailText;
						}

						if (strChildAvail.Contains("R") || strChildAvail.Contains("F"))
						{
							if (strAvailText != "F")
								strAvailText = strChildAvail.Substring(objChild.Avail.Length - 1);
							intAvail += Convert.ToInt32(strChildAvail.Replace("F", string.Empty).Replace("R", string.Empty));
						}
						else
							intAvail += Convert.ToInt32(strChildAvail);
					}
				}

				// Avail cannot go below 0. This typically happens when an item with Avail 0 is given the Second Hand category.
				if (intAvail < 0)
					intAvail = 0;

				strReturn = intAvail.ToString() + strAvailText;

				// Translate the Avail string.
				strReturn = strReturn.Replace("R", LanguageManager.Instance.GetString("String_AvailRestricted"));
				strReturn = strReturn.Replace("F", LanguageManager.Instance.GetString("String_AvailForbidden"));

				return strReturn;
			}
		}

		/// <summary>
		/// Caculated Capacity of the Cyberware.
		/// </summary>
		public string CalculatedCapacity
		{
			get
			{
				if (_strCapacity.Contains("/["))
				{
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					int intPos = _strCapacity.IndexOf("/[");
					string strFirstHalf = _strCapacity.Substring(0, intPos);
					string strSecondHalf = _strCapacity.Substring(intPos + 1, _strCapacity.Length - intPos - 1);
					bool blnSquareBrackets = false;
					string strCapacity = "";

					try
					{
						blnSquareBrackets = strFirstHalf.Contains('[');
						strCapacity = strFirstHalf;
						if (blnSquareBrackets)
							strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
					}
					catch
					{
					}
					XPathExpression xprCapacity = nav.Compile(strCapacity.Replace("Rating", _intRating.ToString()));

					string strReturn = "";
					try
					{
						if (_strCapacity == "[*]")
							strReturn = "*";
						else
						{
							if (_strCapacity.StartsWith("FixedValues"))
                                strReturn = FixedValue(_strCapacity);
							else
								strReturn = nav.Evaluate(xprCapacity).ToString();
						}
						if (blnSquareBrackets)
							strReturn = "[" + strCapacity + "]";
					}
					catch
					{
						strReturn = "0";
					}

					if (strSecondHalf.Contains("Rating"))
					{
						strSecondHalf = strSecondHalf.Replace("[", string.Empty).Replace("]", string.Empty);
						xprCapacity = nav.Compile(strSecondHalf.Replace("Rating", _intRating.ToString()));
						strSecondHalf = "[" + nav.Evaluate(xprCapacity).ToString() + "]";
					}

					strReturn += "/" + strSecondHalf;
					return strReturn;
				}
				else if (_strCapacity.Contains("Rating"))
				{
					// If the Capaicty is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					// XPathExpression cannot evaluate while there are square brackets, so remove them if necessary.
					bool blnSquareBrackets = _strCapacity.Contains('[');
					string strCapacity = _strCapacity;
					if (blnSquareBrackets)
						strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
					XPathExpression xprCapacity = nav.Compile(strCapacity.Replace("Rating", _intRating.ToString()));

					string strReturn = nav.Evaluate(xprCapacity).ToString();
					if (blnSquareBrackets)
						strReturn = "[" + strReturn + "]";
					
					return strReturn;
				}
				else
				{
					if (_strCapacity.StartsWith("FixedValues"))
                        return FixedValue(_strCapacity);
					else
					{
						// Just a straight Capacity, so return the value.
						return _strCapacity;
					}
				}
			}
		}

		/// <summary>
		/// Calculated Essence cost of the Cyberware.
		/// </summary>
		public decimal CalculatedESS
		{
			get
			{
				decimal decReturn = 0;
				decimal decESSMultiplier = 0;

				if (_strESS.Contains("Rating"))
				{
					// If the cost is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strEss = "";
					string strEssExpression = _strESS;

					strEss = strEssExpression.Replace("Rating", _intRating.ToString());
					XPathExpression xprEss = nav.Compile(strEss);
					decReturn = Convert.ToDecimal(nav.Evaluate(xprEss), GlobalOptions.Instance.CultureInfo);
				}
				else
				{
					if (_strESS.StartsWith("FixedValues"))
						decReturn = Convert.ToDecimal(FixedValue(_strESS), GlobalOptions.Instance.CultureInfo);
					else
					{
						// Just a straight cost, so return the value.
						decReturn = Convert.ToDecimal(_strESS, GlobalOptions.Instance.CultureInfo);
					}
				}

				// Factor in the Essence multiplier of the selected CyberwareGrade.
				decESSMultiplier = Grade.Essence;

				if (_blnSuite)
					decESSMultiplier -= 0.1m;

				if (_intEssenceDiscount != 0)
				{
					decimal decDiscount = Convert.ToDecimal(_intEssenceDiscount, GlobalOptions.Instance.CultureInfo) * 0.01m;
					decESSMultiplier -= decDiscount;
				}

				ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);

				// Retrieve the Bioware or Cyberware ESS Cost Multiplier. Bioware Modifiers do not apply to Genetech.
				double dblMultiplier = 1;
				double dblCharacterESSMultiplier = 1;
				double dblBasicBiowareESSMultiplier = 1;
				// Apply the character's Cyberware Essence cost multiplier if applicable.
				if (objImprovementManager.ValueOf(Improvement.ImprovementType.CyberwareEssCost) != 0 && _objImprovementSource == Improvement.ImprovementSource.Cyberware)
				{
					foreach (Improvement objImprovement in _objCharacter.Improvements)
					{
						if (objImprovement.ImproveType == Improvement.ImprovementType.CyberwareEssCost && objImprovement.Enabled)
							dblMultiplier -= (1 - (Convert.ToDouble(objImprovement.Value, GlobalOptions.Instance.CultureInfo) / 100));
					}
					dblCharacterESSMultiplier = dblMultiplier;
				}

				// Apply the character's Bioware Essence cost multiplier if applicable.
				if (objImprovementManager.ValueOf(Improvement.ImprovementType.BiowareEssCost) != 0 && _objImprovementSource == Improvement.ImprovementSource.Bioware)
				{
					foreach (Improvement objImprovement in _objCharacter.Improvements)
					{
						if (objImprovement.ImproveType == Improvement.ImprovementType.BiowareEssCost && objImprovement.Enabled)
							dblMultiplier -= (1 - (Convert.ToDouble(objImprovement.Value, GlobalOptions.Instance.CultureInfo) / 100));
					}
					dblCharacterESSMultiplier = dblMultiplier;
				}

				// Apply the character's Basic Bioware Essence cost multiplier if applicable.
				if (objImprovementManager.ValueOf(Improvement.ImprovementType.BasicBiowareEssCost) != 0 && _objImprovementSource == Improvement.ImprovementSource.Bioware)
				{
					double dblBasicMultiplier = 1;
					foreach (Improvement objImprovement in _objCharacter.Improvements)
					{
						if (objImprovement.ImproveType == Improvement.ImprovementType.BasicBiowareEssCost && objImprovement.Enabled)
							dblBasicMultiplier -= (1 - (Convert.ToDouble(objImprovement.Value, GlobalOptions.Instance.CultureInfo) / 100));
					}
					dblBasicBiowareESSMultiplier = dblBasicMultiplier;
				}

				if (_strCategory.StartsWith("Genetech") || _strCategory.StartsWith("Genetic Infusions"))
					dblCharacterESSMultiplier = 1;

				if (_strCategory == "Basic")
					dblCharacterESSMultiplier -= (1 - dblBasicBiowareESSMultiplier);

				dblCharacterESSMultiplier -= (1 - Convert.ToDouble(decESSMultiplier, GlobalOptions.Instance.CultureInfo));

				decReturn = decReturn * Convert.ToDecimal(dblCharacterESSMultiplier, GlobalOptions.Instance.CultureInfo);

				// Check if the character has Sensitive System.
				if (_objImprovementSource == Improvement.ImprovementSource.Cyberware)
				{
					try
					{
						foreach (Improvement objImprovement in _objCharacter.Improvements)
						{
							if (objImprovement.ImproveType == Improvement.ImprovementType.SensitiveSystem && objImprovement.Enabled)
								decReturn *= 2.0m;
						}
					}
					catch
					{
						// The try/catch block is here because Cyberware plugins can be added to the Mechanical Arms of Vehicles which does not affect the character itself.
					}
				}

				decReturn = Math.Round(decReturn, _objCharacter.Options.EssenceDecimals, MidpointRounding.AwayFromZero);

				return decReturn;
			}
		}

		/// <summary>
		/// Total cost of the Cyberware and its plugins.
		/// </summary>
		public int TotalCost
		{
			get
			{
				int intCost = 0;
				int intReturn = 0;

				if (_strCost.Contains("Rating"))
				{
					// If the cost is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strCost = "";
					string strCostExpression = _strCost;

					strCost = strCostExpression.Replace("Rating", _intRating.ToString());
					XPathExpression xprCost = nav.Compile(strCost);
					intCost = Convert.ToInt32(nav.Evaluate(xprCost).ToString());
				}
				else
				{
					if (_strCost.StartsWith("FixedValues"))
                        intCost = Convert.ToInt32(FixedValue(_strCost));
					else
					{
						// Just a straight cost, so return the value.
						try
						{
							intCost = Convert.ToInt32(_strCost);
						}
						catch
						{
							intCost = 0;
						}
					}
				}

				// Factor in the Cost multiplier of the selected CyberwareGrade.
				intCost = Convert.ToInt32(Convert.ToDouble(intCost, GlobalOptions.Instance.CultureInfo) * Grade.Cost);

				intReturn = intCost;

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				// Add in the cost of all child components.
				foreach (Cyberware objChild in Cyberwares)
				{
					if (objChild.Capacity != "[*]")
					{
						// If the child cost starts with "*", multiply the item's base cost.
						if (objChild.Cost.StartsWith("*"))
						{
							int intPluginCost = 0;
							string strMultiplier = objChild.Cost;
							strMultiplier = strMultiplier.Replace("*", string.Empty);
							intPluginCost = Convert.ToInt32(intCost * (Convert.ToDouble(strMultiplier, GlobalOptions.Instance.CultureInfo) - 1));

							if (objChild.DiscountCost)
								intPluginCost = Convert.ToInt32(Convert.ToDouble(intPluginCost, GlobalOptions.Instance.CultureInfo) * 0.9);
							
							intReturn += intPluginCost;
						}
						else
							intReturn += objChild.TotalCostWithoutModifiers;
					}

					// Add in the cost of any Plugin Gear plugins.
					foreach (Gear objGear in objChild.Gears)
						intReturn += objGear.TotalCost;
				}

				// Add in the cost of all Gear plugins.
				foreach (Gear objGear in Gears)
				{
					intReturn += objGear.TotalCost;
				}

				// Retrieve the Genetech Cost Multiplier if available.
				double dblMultiplier = 1;
				ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
				if (objImprovementManager.ValueOf(Improvement.ImprovementType.GenetechCostMultiplier) != 0 && _objImprovementSource == Improvement.ImprovementSource.Bioware && _strCategory.StartsWith("Genetech"))
				{
					foreach (Improvement objImprovement in _objCharacter.Improvements)
					{
						if (objImprovement.ImproveType == Improvement.ImprovementType.GenetechCostMultiplier && objImprovement.Enabled)
							dblMultiplier -= (1 - (Convert.ToDouble(objImprovement.Value, GlobalOptions.Instance.CultureInfo) / 100));
					}
				}

				// Retrieve the Transgenics Cost Multiplier if available.
				if (objImprovementManager.ValueOf(Improvement.ImprovementType.TransgenicsBiowareCost) != 0 && _objImprovementSource == Improvement.ImprovementSource.Bioware && _strCategory == "Genetech: Transgenics")
				{
					foreach (Improvement objImprovement in _objCharacter.Improvements)
					{
						if (objImprovement.ImproveType == Improvement.ImprovementType.TransgenicsBiowareCost && objImprovement.Enabled)
							dblMultiplier -= (1 - (Convert.ToDouble(objImprovement.Value, GlobalOptions.Instance.CultureInfo) / 100));
					}
				}

				if (dblMultiplier == 0)
					dblMultiplier = 1;

				double dblSuiteMultiplier = 1.0;
				if (_blnSuite)
					dblSuiteMultiplier = 0.9;

				return Convert.ToInt32(Math.Round((Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * Convert.ToDouble(dblMultiplier, GlobalOptions.Instance.CultureInfo) * dblSuiteMultiplier), 2, MidpointRounding.AwayFromZero));
			}
		}

		/// <summary>
		/// Identical to TotalCost, but without the Improvement and Suite multpliers which would otherwise be doubled.
		/// </summary>
		private int TotalCostWithoutModifiers
		{
			get
			{
				int intCost = 0;
				int intReturn = 0;

				if (_strCost.Contains("Rating"))
				{
					// If the cost is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strCost = "";
					string strCostExpression = _strCost;

					strCost = strCostExpression.Replace("Rating", _intRating.ToString());
					XPathExpression xprCost = nav.Compile(strCost);
					intCost = Convert.ToInt32(nav.Evaluate(xprCost).ToString());
				}
				else
				{
					if (_strCost.StartsWith("FixedValues"))
                        intCost = Convert.ToInt32(FixedValue(_strCost));
					else
					{
						// Just a straight cost, so return the value.
						try
						{
							intCost = Convert.ToInt32(_strCost);
						}
						catch
						{
							intCost = 0;
						}
					}
				}

				// Factor in the Cost multiplier of the selected CyberwareGrade.
				intCost = Convert.ToInt32(Convert.ToDouble(intCost, GlobalOptions.Instance.CultureInfo) * Grade.Cost);

				intReturn = intCost;

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				const double dblMultiplier = 1;
				const double decSuiteMultiplier = 1.0;

				return Convert.ToInt32(Math.Round((Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * Convert.ToDouble(dblMultiplier, GlobalOptions.Instance.CultureInfo) * decSuiteMultiplier), 2, MidpointRounding.AwayFromZero));
			}
		}

		/// <summary>
		/// Cost of just the Cyberware itself.
		/// </summary>
		public int OwnCost
		{
			get
			{
				int intCost = 0;
				int intReturn = 0;

				if (_strCost.Contains("Rating"))
				{
					// If the cost is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strCost = "";
					string strCostExpression = _strCost;

					strCost = strCostExpression.Replace("Rating", _intRating.ToString());
					XPathExpression xprCost = nav.Compile(strCost);
					intCost = Convert.ToInt32(nav.Evaluate(xprCost).ToString());
				}
				else
				{
					if (_strCost.StartsWith("FixedValues"))
                        intCost = Convert.ToInt32(FixedValue(_strCost));
					else
					{
						// Just a straight cost, so return the value.
						try
						{
							intCost = Convert.ToInt32(_strCost);
						}
						catch
						{
							intCost = 0;
						}
					}
				}

				// Factor in the Cost multiplier of the selected CyberwareGrade.
				intCost = Convert.ToInt32(Convert.ToDouble(intCost, GlobalOptions.Instance.CultureInfo) * Grade.Cost);

				intReturn = intCost;

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				// Retrieve the Genetech Cost Multiplier if available.
				double dblMultiplier = 1;
				ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
				if (objImprovementManager.ValueOf(Improvement.ImprovementType.GenetechCostMultiplier) != 0 && _objImprovementSource == Improvement.ImprovementSource.Bioware && _strCategory.StartsWith("Genetech"))
				{
					foreach (Improvement objImprovement in _objCharacter.Improvements)
					{
						if (objImprovement.ImproveType == Improvement.ImprovementType.GenetechCostMultiplier && objImprovement.Enabled)
							dblMultiplier -= (1 - (Convert.ToDouble(objImprovement.Value, GlobalOptions.Instance.CultureInfo) / 100));
					}
				}

				// Retrieve the Transgenics Cost Multiplier if available.
				if (objImprovementManager.ValueOf(Improvement.ImprovementType.TransgenicsBiowareCost) != 0 && _objImprovementSource == Improvement.ImprovementSource.Bioware && _strCategory == "Genetech: Transgenics")
				{
					foreach (Improvement objImprovement in _objCharacter.Improvements)
					{
						if (objImprovement.ImproveType == Improvement.ImprovementType.TransgenicsBiowareCost && objImprovement.Enabled)
							dblMultiplier -= (1 - (Convert.ToDouble(objImprovement.Value, GlobalOptions.Instance.CultureInfo) / 100));
					}
				}

				if (dblMultiplier == 0)
					dblMultiplier = 1;

				double dblSuiteMultiplier = 1.0;
				if (_blnSuite)
					dblSuiteMultiplier = 0.9;

				return Convert.ToInt32(Math.Round((Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * Convert.ToDouble(dblMultiplier, GlobalOptions.Instance.CultureInfo) * dblSuiteMultiplier), 2, MidpointRounding.AwayFromZero));
			}
		}

		/// <summary>
		/// The amount of Capacity remaining in the Cyberware.
		/// </summary>
		public int CapacityRemaining
		{
			get
			{
				int intCapacity = 0;
				if (_strCapacity.Contains("/["))
				{
					// Get the Cyberware base Capacity.
					string strBaseCapacity = CalculatedCapacity;
					strBaseCapacity = strBaseCapacity.Substring(0, strBaseCapacity.IndexOf('/'));
					intCapacity = Convert.ToInt32(strBaseCapacity);

					// Run through its Children and deduct the Capacity costs.
					foreach (Cyberware objChildCyberware in Cyberwares)
					{
						string strCapacity = objChildCyberware.CalculatedCapacity;
						if (strCapacity.Contains("/["))
							strCapacity = strCapacity.Substring(strCapacity.IndexOf('[') + 1, strCapacity.IndexOf(']') - strCapacity.IndexOf('[') - 1);
						else if (strCapacity.Contains("["))
							strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
						if (strCapacity == "*")
							strCapacity = "0";
						intCapacity -= Convert.ToInt32(strCapacity);
					}

				}
				else if (!_strCapacity.Contains("["))
				{
					// Get the Cyberware base Capacity.
					intCapacity = Convert.ToInt32(CalculatedCapacity);

					// Run through its Children and deduct the Capacity costs.
					foreach (Cyberware objChildCyberware in Cyberwares)
					{
						string strCapacity = objChildCyberware.CalculatedCapacity;
						if (strCapacity.Contains("/["))
							strCapacity = strCapacity.Substring(strCapacity.IndexOf('[') + 1, strCapacity.IndexOf(']') - strCapacity.IndexOf('[') - 1);
						else if (strCapacity.Contains("["))
							strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
						if (strCapacity == "*")
							strCapacity = "0";
						intCapacity -= Convert.ToInt32(strCapacity);
					}
				}
				
				return intCapacity;
			}
		}

		/// <summary>
		/// Cyberlimb Strength.
		/// </summary>
		public int TotalStrength
		{
			get
			{
				if (_strCategory != "Cyberlimb")
					return 0;

				// Base Strength for any limb is 3.
				int intAttribute = 3;
				int intBonus = 0;

				foreach (Cyberware objChild in Cyberwares)
				{
					// If the limb has Customized Strength, this is its new base value.
					if (objChild.Name == "Customized Strength")
						intAttribute = objChild.Rating;
					// If the limb has Enhanced Strength, this adds to the limb's value.
					if (objChild.Name == "Enhanced Strength")
						intBonus = objChild.Rating;
				}

				return intAttribute + intBonus;
			}
		}

		/// <summary>
		/// Cyberlimb Body.
		/// </summary>
		public int TotalBody
		{
			get
			{
				if (_strCategory != "Cyberlimb")
					return 0;

				// Base Strength for any limb is 3.
				int intAttribute = 3;
				int intBonus = 0;

				foreach (Cyberware objChild in Cyberwares)
				{
					// If the limb has Customized Body, this is its new base value.
					if (objChild.Name == "Customized Body")
						intAttribute = objChild.Rating;
					// If the limb has Enhanced Body, this adds to the limb's value.
					if (objChild.Name == "Enhanced Body")
						intBonus = objChild.Rating;
				}

				return intAttribute + intBonus;
			}
		}

		/// <summary>
		/// Cyberlimb Agility.
		/// </summary>
		public int TotalAgility
		{
			get
			{
				if (_strCategory != "Cyberlimb")
					return 0;

				// Base Strength for any limb is 3.
				int intAttribute = 3;
				int intBonus = 0;

				foreach (Cyberware objChild in Cyberwares)
				{
					// If the limb has Customized Agility, this is its new base value.
					if (objChild.Name == "Customized Agility")
						intAttribute = objChild.Rating;
					// If the limb has Enhanced Agility, this adds to the limb's value.
					if (objChild.Name == "Enhanced Agility")
						intBonus = objChild.Rating;
				}

				return intAttribute + intBonus;
			}
		}

		/// <summary>
		/// Whether or not the Cyberware is allowed to accept Modular Plugins.
		/// </summary>
		public bool AllowModularPlugins
		{
			get
			{
				bool blnReturn = false;
				foreach (Cyberware objChild in Cyberwares)
				{
					if (objChild.Subsytems.Contains("Modular Plug-In"))
					{
						blnReturn = true;
						break;
					}
				}

				return blnReturn;
			}
		}
		#endregion
	}

	/// <summary>
	/// A Weapon.
	/// </summary>
	public class Weapon : Equipment
	{
		private string _strType = "";
		private int _intReach = 0;
		private string _strDamage = "";
		private string _strAP = "0";
		private string _strMode = "";
		private string _strRC = "";
		private string _strAmmo = "";
		private string _strAmmoCategory = "";
		private int _intConceal = 0;
		private int _intAmmoRemaining = 0;
		private int _intAmmoRemaining2 = 0;
		private int _intAmmoRemaining3 = 0;
		private int _intAmmoRemaining4 = 0;
		private Guid _guiAmmoLoaded = new Guid();
		private Guid _guiAmmoLoaded2 = new Guid();
		private Guid _guiAmmoLoaded3 = new Guid();
		private Guid _guiAmmoLoaded4 = new Guid();
		private int _intActiveAmmoSlot = 1;
		private string _strRange = "";
		private double _dblRangeMultiplier = 1;
		private int _intFullBurst = 10;
		private int _intSuppressive = 20;
		private bool _blnUnderbarrel = false;
		private bool _blnVehicleMounted = false;
		private string _strUseSkill = "";
		private string _strLocation = "";
		private string _strSpec = "";
		private string _strSpec2 = "";
		private bool _blnRequireAmmo = true;

		#region Constructor, Create, Save, Load, and Print Methods
		public Weapon(Character objCharacter) : base(objCharacter)
		{
		}

		/// Create a Weapon from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlWeapon">XmlNode to create the object from.</param>
		/// <param name="objCharacter">Character that the Weapon is being added to.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		/// <param name="cmsWeapon">ContextMenuStrip to use for Weapons.</param>
		/// <param name="cmsWeaponAccessory">ContextMenuStrip to use for Accessories.</param>
		/// <param name="cmsWeaponMod">ContextMenuStrip to use for Weapon Mods.</param>
		/// <param name="blnCreateChildren">Whether or not child items should be created.</param>
		public void Create(XmlNode objXmlWeapon, Character objCharacter, TreeNode objNode, ContextMenuStrip cmsWeapon, ContextMenuStrip cmsWeaponAccessory, ContextMenuStrip cmsWeaponMod, bool blnCreateChildren = true)
		{
            _guidExternalID = Guid.Parse(objXmlWeapon["id"].InnerText);
			_strName = objXmlWeapon["name"].InnerText;
			_strCategory = objXmlWeapon["category"].InnerText;
			_strType = objXmlWeapon["type"].InnerText;
			_intReach = Convert.ToInt32(objXmlWeapon["reach"].InnerText);
			_strDamage = objXmlWeapon["damage"].InnerText;
			_strAP = objXmlWeapon["ap"].InnerText;
			_strMode = objXmlWeapon["mode"].InnerText;
			_strAmmo = objXmlWeapon["ammo"].InnerText;
            _strAccuracy = objXmlWeapon["accuracy"].InnerText;
			if (objXmlWeapon["ammocategory"] != null)
				_strAmmoCategory = objXmlWeapon["ammocategory"].InnerText;
			_strRC = objXmlWeapon["rc"].InnerText;
            if (objXmlWeapon["conceal"] != null)
				_intConceal = Convert.ToInt32(objXmlWeapon["conceal"].InnerText);
			_strAvail = objXmlWeapon["avail"].InnerText;
			_strSource = objXmlWeapon["source"].InnerText;
			_strPage = objXmlWeapon["page"].InnerText;

			XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlNode objWeaponNode = objXmlDocument.SelectSingleNode("/chummer/weapons/weapon[id = \"" + ExternalId + "\"]");
				if (objWeaponNode != null)
				{
					if (objWeaponNode["translate"] != null)
						_strAltName = objWeaponNode["translate"].InnerText;
					if (objWeaponNode["altpage"] != null)
						_strAltPage = objWeaponNode["altpage"].InnerText;
				}

				objWeaponNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objWeaponNode != null)
				{
					if (objWeaponNode.Attributes["translate"] != null)
						_strAltCategory = objWeaponNode.Attributes["translate"].InnerText;
				}
			}

            // Check for a Variable Cost.
            if (objXmlWeapon["cost"].InnerText.StartsWith("Variable"))
                _strCost = VariableCost(objXmlWeapon["cost"].InnerText, DisplayNameShort).ToString();
            else
                _strCost = objXmlWeapon["cost"].InnerText;

			// Populate the Range if it differs from the Weapon's Category.
			if (objXmlWeapon["range"] != null)
			{
				_strRange = objXmlWeapon["range"].InnerText;
				if (objXmlWeapon["range"].Attributes["multiply"] != null)
					_dblRangeMultiplier = Convert.ToDouble(objXmlWeapon["range"].Attributes["multiply"].InnerText, GlobalOptions.Instance.CultureInfo);
			}

			if (objXmlWeapon["fullburst"] != null)
				_intFullBurst = Convert.ToInt32(objXmlWeapon["fullburst"].InnerText);
            if (objXmlWeapon["suppressive"] != null)
				_intSuppressive = Convert.ToInt32(objXmlWeapon["suppressive"].InnerText);

			if (objXmlWeapon["useskill"] != null)
				_strUseSkill = objXmlWeapon["useskill"].InnerText;

			if (objXmlWeapon["requireammo"] != null)
				_blnRequireAmmo = Convert.ToBoolean(objXmlWeapon["requireammo"].InnerText);

			if (objXmlWeapon["spec"] != null)
				_strSpec = objXmlWeapon["spec"].InnerText;

			if (objXmlDocument["spec2"] != null)
				_strSpec2 = objXmlWeapon["spec2"].InnerText;

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();

			// If the Weapon comes with an Underbarrel Weapon, add it.
			if (objXmlWeapon["underbarrel"] != null && blnCreateChildren)
			{
				XmlNode objXmlUnderbarrel = objXmlWeapon.SelectSingleNode("underbarrel");
				Weapon objUnderbarrelWeapon = new Weapon(_objCharacter);
				TreeNode objUnderbarrelNode = new TreeNode();
				XmlNode objXmlWeaponNode = objXmlDocument.SelectSingleNode("/chummer/weapons/weapon[id = \"" + objXmlUnderbarrel.InnerText + "\"]");
				objUnderbarrelWeapon.Create(objXmlWeaponNode, _objCharacter, objUnderbarrelNode, cmsWeapon, cmsWeaponAccessory, cmsWeaponMod);
				objUnderbarrelWeapon.IncludedInParent = true;
				objUnderbarrelWeapon.IsUnderbarrelWeapon = true;
				Weapons.Add(objUnderbarrelWeapon);
				objUnderbarrelNode.ContextMenuStrip = cmsWeapon;
				objNode.Nodes.Add(objUnderbarrelNode);
			}

			// If there are any Accessories that come with the Weapon, add them.
			if (objXmlWeapon["accessories"] != null && blnCreateChildren)
			{
				XmlNodeList objXmlAccessoryList = objXmlWeapon.SelectNodes("accessories/id");
				foreach (XmlNode objXmlWeaponAccessory in objXmlAccessoryList)
				{
					XmlNode objXmlAccessory = objXmlDocument.SelectSingleNode("/chummer/accessories/accessory[id = \"" + objXmlWeaponAccessory.InnerText + "\"]");
					TreeNode objAccessoryNode = new TreeNode();
					WeaponAccessory objAccessory = new WeaponAccessory(_objCharacter);
					objAccessory.Create(objXmlAccessory, objAccessoryNode, objXmlAccessory["mount"].InnerText);
					objAccessory.IncludedInParent = true;
					objAccessory.Parent = this;
					objAccessoryNode.ContextMenuStrip = cmsWeaponAccessory;
					WeaponAccessories.Add(objAccessory);

					objNode.Nodes.Add(objAccessoryNode);
					objNode.Expand();
				}
			}

			// If there are any Mods that come with the Weapon, add them.
			if (objXmlWeapon["mods"] != null && blnCreateChildren)
			{
				XmlNodeList objXmlModList = objXmlWeapon.SelectNodes("mods/id");
				foreach (XmlNode objXmlWeaponMod in objXmlModList)
				{
					TreeNode objModNode = new TreeNode();
					XmlNode objXmlMod = objXmlDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + objXmlWeaponMod.InnerText + "\"]");
					WeaponMod objMod = new WeaponMod(_objCharacter);
					objMod.Create(objXmlMod, objModNode);
					objMod.IncludedInParent = true;
					objMod.Parent = this;
					objModNode.ContextMenuStrip = cmsWeaponMod;
					WeaponMods.Add(objMod);

					if (objXmlWeaponMod.Attributes["rating"] != null)
						objMod.Rating = Convert.ToInt32(objXmlWeaponMod.Attributes["rating"].InnerText);

					objModNode.Text = objMod.DisplayName;

					objNode.Nodes.Add(objModNode);
					objNode.Expand();
				}
			}
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("weapon");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("category", _strCategory);
			objWriter.WriteElementString("type", _strType);
			objWriter.WriteElementString("spec", _strSpec);
			objWriter.WriteElementString("spec2", _strSpec2);
			objWriter.WriteElementString("reach", _intReach.ToString());
			objWriter.WriteElementString("damage", _strDamage);
			objWriter.WriteElementString("ap", _strAP);
			objWriter.WriteElementString("mode", _strMode);
			objWriter.WriteElementString("rc", _strRC);
			objWriter.WriteElementString("accuracy", _strAccuracy.ToString());
			objWriter.WriteElementString("ammo", _strAmmo);
			objWriter.WriteElementString("ammocategory", _strAmmoCategory);
			objWriter.WriteElementString("ammoremaining", _intAmmoRemaining.ToString());
			objWriter.WriteElementString("ammoremaining2", _intAmmoRemaining2.ToString());
			objWriter.WriteElementString("ammoremaining3", _intAmmoRemaining3.ToString());
			objWriter.WriteElementString("ammoremaining4", _intAmmoRemaining4.ToString());
			objWriter.WriteElementString("ammoloaded", _guiAmmoLoaded.ToString());
			objWriter.WriteElementString("ammoloaded2", _guiAmmoLoaded2.ToString());
			objWriter.WriteElementString("ammoloaded3", _guiAmmoLoaded3.ToString());
			objWriter.WriteElementString("ammoloaded4", _guiAmmoLoaded4.ToString());
			objWriter.WriteElementString("conceal", _intConceal.ToString());
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("useskill", _strUseSkill);
			objWriter.WriteElementString("range", _strRange);
			objWriter.WriteElementString("rangemultiply", _dblRangeMultiplier.ToString(GlobalOptions.Instance.CultureInfo));
			objWriter.WriteElementString("fullburst", _intFullBurst.ToString());
			objWriter.WriteElementString("suppressive", _intSuppressive.ToString());
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("included", _blnIncludedInParent.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("requireammo", _blnRequireAmmo.ToString());
			if (WeaponAccessories.Count > 0)
			{
				objWriter.WriteStartElement("accessories");
				foreach (WeaponAccessory objAccessory in WeaponAccessories)
					objAccessory.Save(objWriter);
				objWriter.WriteEndElement();
			}
			if (WeaponMods.Count > 0)
			{
				objWriter.WriteStartElement("weaponmods");
				foreach (WeaponMod objMod in WeaponMods)
					objMod.Save(objWriter);
				objWriter.WriteEndElement();
			}
			if (Weapons.Count > 0)
			{
				foreach (Weapon objUnderbarrel in Weapons)
				{
					objWriter.WriteStartElement("underbarrel");
					objUnderbarrel.Save(objWriter);
					objWriter.WriteEndElement();
				}
			}
			objWriter.WriteElementString("location", _strLocation);
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Attribute from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public override void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			if (objNode["category"].InnerText == "Hold-Outs")
				objNode["category"].InnerText = "Holdouts";
			if (objNode["category"].InnerText == "Cyberware Hold-Outs")
				objNode["category"].InnerText = "Cyberware Holdouts";
			_strCategory = objNode["category"].InnerText;
			_strType = objNode["type"].InnerText;
			_strSpec = objNode["spec"].InnerText;
			_strSpec2 = objNode["spec2"].InnerText;
			_intReach = Convert.ToInt32(objNode["reach"].InnerText);
			_strDamage = objNode["damage"].InnerText;
			_strAP = objNode["ap"].InnerText;
			_strMode = objNode["mode"].InnerText;
			_strRC = objNode["rc"].InnerText;
			_strAccuracy = objNode["accuracy"].InnerText;
			_strAmmo = objNode["ammo"].InnerText;
			_strAmmoCategory = objNode["ammocategory"].InnerText;
			_intAmmoRemaining = Convert.ToInt32(objNode["ammoremaining"].InnerText);
			_intAmmoRemaining2 = Convert.ToInt32(objNode["ammoremaining2"].InnerText);
			_intAmmoRemaining3 = Convert.ToInt32(objNode["ammoremaining3"].InnerText);
			_intAmmoRemaining4 = Convert.ToInt32(objNode["ammoremaining4"].InnerText);
			_guiAmmoLoaded = Guid.Parse(objNode["ammoloaded"].InnerText);
			_guiAmmoLoaded2 = Guid.Parse(objNode["ammoloaded2"].InnerText);
			_guiAmmoLoaded3 = Guid.Parse(objNode["ammoloaded3"].InnerText);
			_guiAmmoLoaded4 = Guid.Parse(objNode["ammoloaded4"].InnerText);
			_intConceal = Convert.ToInt32(objNode["conceal"].InnerText);
			_strAvail = objNode["avail"].InnerText;
			_strCost = objNode["cost"].InnerText;
			_intFullBurst = Convert.ToInt32(objNode["fullburst"].InnerText);
			_intSuppressive = Convert.ToInt32(objNode["suppressive"].InnerText);
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;
			_strGivenName = objNode["givenname"].InnerText;

            try
			{
				if (objNode["range"].InnerText == "Hold-Outs")
					objNode["range"].InnerText = "Holdouts";
				_strRange = objNode["range"].InnerText;
			}
			catch
			{
			}

			_strUseSkill = objNode["useskill"].InnerText;
			_dblRangeMultiplier = Convert.ToDouble(objNode["rangemultiply"].InnerText, GlobalOptions.Instance.CultureInfo);
			_blnIncludedInParent = Convert.ToBoolean(objNode["included"].InnerText);
			_blnEquipped = Convert.ToBoolean(objNode["equipped"].InnerText);
			_blnRequireAmmo = Convert.ToBoolean(objNode["requireammo"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");
				XmlNode objWeaponNode = objXmlDocument.SelectSingleNode("/chummer/weapons/weapon[id = \"" + ExternalId + "\"]");
				if (objWeaponNode != null)
				{
					if (objWeaponNode["translate"] != null)
						_strAltName = objWeaponNode["translate"].InnerText;
					if (objWeaponNode["altpage"] != null)
						_strAltPage = objWeaponNode["altpage"].InnerText;
				}

				objWeaponNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objWeaponNode != null)
				{
					if (objWeaponNode.Attributes["translate"] != null)
						_strAltCategory = objWeaponNode.Attributes["translate"].InnerText;
				}
			}

			if (objNode["accessories"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("accessories/accessory");
				foreach (XmlNode nodChild in nodChildren)
				{
					WeaponAccessory objAccessory = new WeaponAccessory(_objCharacter);
					objAccessory.Load(nodChild, blnCopy);
					objAccessory.Parent = this;
					WeaponAccessories.Add(objAccessory);
				}
			}

			if (objNode["weaponmods"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("weaponmods/weaponmod");
				foreach (XmlNode nodChild in nodChildren)
				{
					WeaponMod objMod = new WeaponMod(_objCharacter);
					objMod.Load(nodChild, blnCopy);
					objMod.Parent = this;
					WeaponMods.Add(objMod);
				}
			}

			if (objNode["underbarrel"] != null)
			{
				foreach (XmlNode nodWeapon in objNode.SelectNodes("underbarrel/weapon"))
				{
					Weapon objUnderbarrel = new Weapon(_objCharacter);
					objUnderbarrel.Load(nodWeapon, blnCopy);
					objUnderbarrel.IsUnderbarrelWeapon = true;
					objUnderbarrel.Parent = this;
					Weapons.Add(objUnderbarrel);
				}
			}

			_strNotes = objNode["notes"].InnerText;
			_strLocation = objNode["location"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
				_guiAmmoLoaded = Guid.Empty;
				_guiAmmoLoaded2 = Guid.Empty;
				_guiAmmoLoaded3 = Guid.Empty;
				_guiAmmoLoaded4 = Guid.Empty;
				_intActiveAmmoSlot = 1;
				_intAmmoRemaining = 0;
				_intAmmoRemaining2 = 0;
				_intAmmoRemaining3 = 0;
				_intAmmoRemaining4 = 0;
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Print(XmlTextWriter objWriter)
		{
			// Find the piece of Gear that created this item if applicable.
			CommonFunctions objFunctions = new CommonFunctions(_objCharacter);
			Gear objGear = objFunctions.FindGearByWeaponID(_guiID.ToString(), _objCharacter.Gear);

			objWriter.WriteStartElement("weapon");
			objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("name_english", _strName);
			objWriter.WriteElementString("category", DisplayCategory);
			objWriter.WriteElementString("category_english", _strCategory);
			objWriter.WriteElementString("type", _strType);
			objWriter.WriteElementString("reach", TotalReach.ToString());
			objWriter.WriteElementString("damage", CalculatedDamage());
			objWriter.WriteElementString("damage_english", CalculatedDamage(0, true));
			objWriter.WriteElementString("rawdamage", _strDamage);
			objWriter.WriteElementString("ap", TotalAP);
			objWriter.WriteElementString("mode", CalculatedMode);
			objWriter.WriteElementString("rc", TotalRC);
			objWriter.WriteElementString("accuracy", _strAccuracy.ToString());
			objWriter.WriteElementString("ammo", CalculatedAmmo());
			objWriter.WriteElementString("ammo_english", CalculatedAmmo(true));
			objWriter.WriteElementString("conceal", CalculatedConcealability());
			if (objGear != null)
			{
				objWriter.WriteElementString("avail", objGear.TotalAvail(true));
				objWriter.WriteElementString("cost", objGear.TotalCost.ToString());
				objWriter.WriteElementString("owncost", objGear.OwnCost.ToString());
			}
			else
			{
				objWriter.WriteElementString("avail", TotalAvail);
				objWriter.WriteElementString("cost", TotalCost.ToString());
				objWriter.WriteElementString("owncost", OwnCost.ToString());
			}
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("location", _strLocation);
			if (WeaponAccessories.Count > 0)
			{
				objWriter.WriteStartElement("accessories");
				foreach (WeaponAccessory objAccessory in WeaponAccessories)
					objAccessory.Print(objWriter);
				objWriter.WriteEndElement();
			}
			if (WeaponMods.Count > 0)
			{
				objWriter.WriteStartElement("mods");
				foreach (WeaponMod objMod in WeaponMods)
					objMod.Print(objWriter);
				objWriter.WriteEndElement();
			}

			// <ranges>
			objWriter.WriteStartElement("ranges");
			objWriter.WriteElementString("short", RangeShort);
			objWriter.WriteElementString("medium", RangeMedium);
			objWriter.WriteElementString("long", RangeLong);
			objWriter.WriteElementString("extreme", RangeExtreme);
			// </ranges>
			objWriter.WriteEndElement();

			if (Weapons.Count > 0)
			{
				foreach (Weapon objUnderbarrel in Weapons)
				{
					objWriter.WriteStartElement("underbarrel");
					objUnderbarrel.Print(objWriter);
					objWriter.WriteEndElement();
				}
			}

			// Currently loaded Ammo.
			Guid guiAmmo = new Guid();
			if (_intActiveAmmoSlot == 1)
				guiAmmo = _guiAmmoLoaded;
			else if (_intActiveAmmoSlot == 2)
				guiAmmo = _guiAmmoLoaded2;
			else if (_intActiveAmmoSlot == 3)
				guiAmmo = _guiAmmoLoaded3;
			else if (_intActiveAmmoSlot == 4)
				guiAmmo = _guiAmmoLoaded4;

			objWriter.WriteElementString("currentammo", GetAmmoName(guiAmmo));
			objWriter.WriteElementString("ammoslot1", GetAmmoName(_guiAmmoLoaded));
			objWriter.WriteElementString("ammoslot2", GetAmmoName(_guiAmmoLoaded2));
			objWriter.WriteElementString("ammoslot3", GetAmmoName(_guiAmmoLoaded3));
			objWriter.WriteElementString("ammoslot4", GetAmmoName(_guiAmmoLoaded4));

			objWriter.WriteElementString("dicepool", DicePool);

			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);

			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Get the name of Ammo from the character or Vehicle.
		/// </summary>
		/// <param name="guiAmmo">InternalId of the Ammo to find.</param>
		private string GetAmmoName(Guid guiAmmo)
		{
			if (guiAmmo == Guid.Empty)
				return "";
			else
			{
				CommonFunctions objFunctions = new CommonFunctions(_objCharacter);
				Gear objAmmo = (Gear)objFunctions.FindEquipment(guiAmmo.ToString(), _objCharacter.Gear, typeof(Gear));
				if (objAmmo == null)
					objAmmo = (Gear)objFunctions.FindEquipment(guiAmmo.ToString(), _objCharacter.Vehicles, typeof(Gear));

				if (objAmmo != null)
					return objAmmo.DisplayNameShort;
				else
					return "";
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Whether or not this Weapon is an Underbarrel Weapon.
		/// </summary>
		public bool IsUnderbarrelWeapon
		{
			get
			{
				return _blnUnderbarrel;
			}
			set
			{
				_blnUnderbarrel = value;
			}
		}

		/// <summary>
		/// Translated Ammo Category.
		/// </summary>
		public string DisplayAmmoCategory
		{
			get
			{
				string strReturn = AmmoCategory;
				// Get the translated name if applicable.
				if (GlobalOptions.Instance.Language != "en-us")
				{
					XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");
					XmlNode objNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
					if (objNode != null)
					{
						if (objNode.Attributes["translate"] != null)
							strReturn = objNode.Attributes["translate"].InnerText;
					}
				}

				return strReturn;
			}
		}

		/// <summary>
		/// Type of Weapon (either Melee or Ranged).
		/// </summary>
		public string WeaponType
		{
			get
			{
				return _strType;
			}
			set
			{
				_strType = value;
			}
		}

		/// <summary>
		/// Reach.
		/// </summary>
		public int Reach
		{
			get
			{
				return _intReach;
			}
			set
			{
				_intReach = value;
			}
		}

		/// <summary>
		/// Damage.
		/// </summary>
		public string Damage
		{
			get
			{
				return _strDamage;
			}
			set
			{
				_strDamage = value;
			}
		}

		/// <summary>
		/// Armor Penetration.
		/// </summary>
		public string AP
		{
			get
			{
				return _strAP;
			}
			set
			{
				_strAP = value;
			}
		}

		/// <summary>
		/// Firing Mode.
		/// </summary>
		public string Mode
		{
			get
			{
				return _strMode;
			}
			set
			{
				_strMode = value;
			}
		}

		/// <summary>
		/// Recoil.
		/// </summary>
		public string RC
		{
			get
			{
				if (_strRC == "0")
					return "-";
				else
					return _strRC;
			}
			set
			{
				_strRC = value;
			}
		}

		/// <summary>
		/// Ammo.
		/// </summary>
		public string Ammo
		{
			get
			{
				return _strAmmo;
			}
			set
			{
				_strAmmo = value;
			}
		}

		/// <summary>
		/// Category of Ammo the Weapon uses.
		/// </summary>
		public string AmmoCategory
		{
			get
			{
				if (_strAmmoCategory != "")
					return _strAmmoCategory;
				else
				{
					if (_strCategory.StartsWith("Cyberware"))
						return _strCategory.Replace("Cyberware", string.Empty).Trim();
					else
						return _strCategory;
				}
			}
		}

		/// <summary>
		/// The number of rounds remaining in the Weapon.
		/// </summary>
		public int AmmoRemaining
		{
			get
			{
				switch (_intActiveAmmoSlot)
				{
					case 1:
						return _intAmmoRemaining;
					case 2:
						return _intAmmoRemaining2;
					case 3:
						return _intAmmoRemaining3;
					case 4:
						return _intAmmoRemaining4;
					default:
						return _intAmmoRemaining;
				}
			}
			set
			{
				switch (_intActiveAmmoSlot)
				{
					case 1:
						_intAmmoRemaining = value;
						break;
					case 2:
						_intAmmoRemaining2 = value;
						break;
					case 3:
						_intAmmoRemaining3 = value;
						break;
					case 4:
						_intAmmoRemaining4 = value;
						break;
					default:
						_intAmmoRemaining = value;
						break;
				}
			}
		}

		/// <summary>
		/// The type of Ammuniation loaded in the Weapon.
		/// </summary>
		public string AmmoLoaded
		{
			get
			{
				switch (_intActiveAmmoSlot)
				{
					case 1:
						return _guiAmmoLoaded.ToString();
					case 2:
						return _guiAmmoLoaded2.ToString();
					case 3:
						return _guiAmmoLoaded3.ToString();
					case 4:
						return _guiAmmoLoaded4.ToString();
					default:
						return _guiAmmoLoaded.ToString();
				}
			}
			set
			{
				switch (_intActiveAmmoSlot)
				{
					case 1:
						_guiAmmoLoaded = Guid.Parse(value);
						break;
					case 2:
						_guiAmmoLoaded2 = Guid.Parse(value);
						break;
					case 3:
						_guiAmmoLoaded3 = Guid.Parse(value);
						break;
					case 4:
						_guiAmmoLoaded4 = Guid.Parse(value);
						break;
					default:
						_guiAmmoLoaded = Guid.Parse(value);
						break;
				}
			}
		}

		/// <summary>
		/// Active Ammo slot number.
		/// </summary>
		public int ActiveAmmoSlot
		{
			get
			{
				return _intActiveAmmoSlot;
			}
			set
			{
				_intActiveAmmoSlot = value;
			}
		}

		/// <summary>
		/// Number of Ammo slots the Weapon has.
		/// </summary>
		public int AmmoSlots
		{
			get
			{
				int intReturn = 1;

				if (CalculatedAmmo().Contains("x"))
				{
					if (CalculatedAmmo().StartsWith("2x"))
						intReturn = 2;
					if (CalculatedAmmo().StartsWith("3x"))
						intReturn = 3;
					if (CalculatedAmmo().StartsWith("4x"))
						intReturn = 4;
				}

				return intReturn;
			}
		}

		/// <summary>
		/// Concealability.
		/// </summary>
		public int Concealability
		{
			get
			{
				return _intConceal;
			}
			set
			{
				_intConceal = value;
			}
		}

		/// <summary>
		/// Cost.
		/// </summary>
		public int Cost
		{
			get
			{
				return Convert.ToInt32(_strCost);
			}
			set
			{
				_strCost = value.ToString();
			}
		}

		/// <summary>
		/// Location.
		/// </summary>
		public string Location
		{
			get
			{
				return _strLocation;
			}
			set
			{
				_strLocation = value;
			}
		}

		/// <summary>
		/// Whether or not the Weapon is mounted on a Vehicle.
		/// </summary>
		public bool VehicleMounted
		{
			get
			{
				return _blnVehicleMounted;
			}
			set
			{
				_blnVehicleMounted = value;
			}
		}

		/// <summary>
		/// Active Skill that should be used with this Weapon instead of the default one.
		/// </summary>
		public string UseSkill
		{
			get
			{
				return _strUseSkill;
			}
			set
			{
				_strUseSkill = value;
			}
		}

		/// <summary>
		/// Whether or not the Weapon requires Ammo to be reloaded.
		/// </summary>
		public bool RequireAmmo
		{
			get
			{
				return _blnRequireAmmo;
			}
			set
			{
				_blnRequireAmmo = value;
			}
		}

		/// <summary>
		/// The Active Skill Specialization that this Weapon uses, in addition to any others it would normally use.
		/// </summary>
		public string Spec
		{
			get
			{
				return _strSpec;
			}
            set
            {
                _strSpec = value;
            }
		}

		/// <summary>
		/// The second Active Skill Specialization that this Weapon uses, in addition to any others it would normally use.
		/// </summary>
		public string Spec2
		{
			get
			{
				return _strSpec2;
			}
		}
		#endregion

		#region Complex Properties
		/// <summary>
		/// Weapon's total Concealability including all Accessories and Modifications.
		/// </summary>
		public string CalculatedConcealability()
		{
			int intReturn = _intConceal;

			foreach (WeaponAccessory objAccessory in WeaponAccessories)
			{
				if (objAccessory.Equipped)
					intReturn += objAccessory.Concealability;
			}

			foreach (WeaponMod objMod in WeaponMods)
			{
				if (objMod.Equipped)
					intReturn += objMod.Concealability;
			}

			// Add +4 for each Underbarrel Weapon installed.
			if (Weapons.Count > 0)
			{
				foreach (Weapon objUnderbarrelWeapon in Weapons)
				{
					if (objUnderbarrelWeapon.Equipped)
						intReturn += 4;
				}
			}

			// Factor in the character's Concealability modifiers.
			ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
			intReturn += objImprovementManager.ValueOf(Improvement.ImprovementType.Concealability);

			string strReturn = "";
			if (intReturn >= 0)
				strReturn = "+" + intReturn.ToString();
			else
				strReturn = intReturn.ToString();

			return strReturn;
		}

		/// <summary>
		/// Weapon's Damage including all Accessories, Modifications, Attributes, and Ammunition.
		/// </summary>
		public string CalculatedDamage(int intUseSTR = 0, bool blnForceEnglish = false)
		{
			string strReturn = "";

			// If the cost is determined by the Rating, evaluate the expression.
			XmlDocument objXmlDocument = new XmlDocument();
			XPathNavigator nav = objXmlDocument.CreateNavigator();

			string strDamage = "";
			string strDamageExpression = _strDamage;
			string strDamageType = "";
			string strDamageExtra = "";
			XPathExpression xprDamage;

			if (_objCharacter != null)
			{
				ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);

				if (_strCategory.StartsWith("Cyberware"))
				{
					int intSTR = _objCharacter.STR.TotalValue;
					// Look to see if this is attached to a Cyberlimb and use its STR instead.
					foreach (Cyberware objCyberware in _objCharacter.Cyberware)
					{
						if (objCyberware.LimbSlot != string.Empty)
						{
							if (objCyberware.WeaponID == InternalId)
								intSTR = objCyberware.TotalStrength;
							else
							{
								foreach (Cyberware objChild in objCyberware.Cyberwares)
								{
									if (objChild.WeaponID == InternalId)
									{
										intSTR = objCyberware.TotalStrength;
										break;
									}
								}
							}
						}
					}

					// A STR value has been passed, so use that instead.
					if (intUseSTR > 0)
						intSTR = intUseSTR;

					if (_strCategory == "Throwing Weapons" || _strUseSkill == "Throwing Weapons")
						intSTR += objImprovementManager.ValueOf(Improvement.ImprovementType.ThrowSTR);

					strDamage = strDamageExpression.Replace("STR", intSTR.ToString());
				}
				else
				{
					int intThrowDV = 0;
					if (_strCategory == "Throwing Weapons" || _strUseSkill == "Throwing Weapons")
						intThrowDV = objImprovementManager.ValueOf(Improvement.ImprovementType.ThrowSTR);

					// A STR value has been passed, so use that instead.
					if (intUseSTR > 0)
						strDamage = strDamageExpression.Replace("STR", (intUseSTR + intThrowDV).ToString());
					else
						strDamage = strDamageExpression.Replace("STR", (_objCharacter.STR.TotalValue + intThrowDV).ToString());
				}
			}
			else
			{
				// If the character is null, this is a vehicle.
				strDamage = strDamageExpression.Replace("STR", intUseSTR.ToString());
			}

			// Evaluate the min expression if there is one.
			if (strDamage.Contains("min") && !strDamage.Contains("mini") && !strDamage.Contains("mine"))
			{
				string strMin = "";
				int intStart = strDamage.IndexOf("min");
				int intEnd = strDamage.IndexOf(")", intStart);
				strMin = strDamage.Substring(intStart, intEnd - intStart + 1);

				string strExpression = strMin;
				strExpression = strExpression.Replace("min", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);

				string[] strValue = strExpression.Split(',');
				strExpression = Math.Min(Convert.ToInt32(strValue[0]), Convert.ToInt32(strValue[1])).ToString();

				strDamage = strDamage.Replace(strMin, strExpression);
			}

			// Place the Damage Type (P or S) into a string and remove it from the expression.
			if (strDamage.Contains("P or S"))
			{
				strDamageType = "P or S";
				strDamage = strDamage.Replace("P or S", string.Empty);
			}
			else
			{
				if (strDamage.Contains("P"))
				{
					strDamageType = "P";
					strDamage = strDamage.Replace("P", string.Empty);
				}
				if (strDamage.Contains("S"))
				{
					strDamageType = "S";
					strDamage = strDamage.Replace("S", string.Empty);
				}
			}
			// Place any extra text like (e) and (f) in a string and remove it from the expression.
			if (strDamage.Contains("(e)"))
			{
				strDamageExtra = "(e)";
				strDamage = strDamage.Replace("(e)", string.Empty);
			}
			if (strDamage.Contains("(f)"))
			{
				strDamageExtra = "(f)";
				strDamage = strDamage.Replace("(f)", string.Empty);
			}

			// Look for splash damage info.
			if (strDamage.Contains("/m)"))
			{
				string strSplash = strDamage.Substring(strDamage.IndexOf("("), strDamage.IndexOf(")") - strDamage.IndexOf("(") + 1);
				strDamageExtra += " " + strSplash;
				strDamage = strDamage.Replace(strSplash, string.Empty).Trim();
			}

			// Replace the division sign with "div" since we're using XPath.
			strDamage = strDamage.Replace("/", " div ");

			// Include WeaponCategoryDV and WepaonSkillDV Improvements.
			int intImprove = 0;
			if (_objCharacter != null)
			{
				foreach (Improvement objImprovement in _objCharacter.Improvements)
				{
					if (objImprovement.ImproveType == Improvement.ImprovementType.WeaponCategoryDV && objImprovement.Enabled && (objImprovement.ImprovedName == _strCategory || "Cyberware " + objImprovement.ImprovedName == _strCategory))
						intImprove += objImprovement.Value;
					if (_strUseSkill != string.Empty)
					{
						if (objImprovement.ImproveType == Improvement.ImprovementType.WeaponCategoryDV && objImprovement.Enabled && (objImprovement.ImprovedName == _strUseSkill || "Cyberware " + objImprovement.ImprovedName == _strCategory))
							intImprove += objImprovement.Value;
					}
                    if (objImprovement.ImproveType == Improvement.ImprovementType.WeaponSkillDV && objImprovement.Enabled && (objImprovement.ImprovedName == ActiveSkill.Name))
                        intImprove += objImprovement.Value;
				}
			}

			// If this is the Unarmed Attack Weapon and the character has the UnarmedDVPhysical Improvement, change the type to Physical.
			// This should also add any UnarmedDV bonus which only applies to Unarmed Combat, not Unarmed Weapons.
			if (ExternalId == Constants.UnarmedAttack)
			{
				foreach (Improvement objImprovement in _objCharacter.Improvements)
				{
					if (objImprovement.ImproveType == Improvement.ImprovementType.UnarmedDVPhysical && objImprovement.Enabled)
						strDamageType = "P";
					if (objImprovement.ImproveType == Improvement.ImprovementType.UnarmedDV && objImprovement.Enabled)
						intImprove += objImprovement.Value;
				}
			}

			// Add in the DV bonus from any Weapon Mods.
			foreach (WeaponMod objMod in WeaponMods)
			{
				if (objMod.Equipped)
					intImprove += objMod.DVBonus;
			}

			strDamage += " + " + intImprove.ToString();

			try
			{
				CommonFunctions objFunctions = new CommonFunctions(_objCharacter);
				CharacterOptions objOptions = _objCharacter.Options;
				int intBonus = 0;
				if (objOptions.MoreLethalGameplay)
					intBonus = 2;
				bool blnDamageReplaced = false;

				// Check if the Weapon has Ammunition loaded and look for any Damage bonus/replacement.
				if (AmmoLoaded != "")
				{
					// Look for Ammo on the character.
					Gear objGear = (Gear)objFunctions.FindEquipment(AmmoLoaded, _objCharacter.Gear, typeof(Gear));
					if (objGear == null)
						objGear = (Gear)objFunctions.FindEquipment(AmmoLoaded, _objCharacter.Vehicles, typeof(Gear));
					if (objGear != null)
					{
						if (objGear.WeaponBonus != null)
						{
							// Change the Weapon's Damage Type. (flechette rounds cannot affect weapons that have flechette included in their damage)
							if (!(objGear.WeaponBonus.InnerXml.Contains("(f)") && _strDamage.Contains("(f)")))
							{
								if (objGear.WeaponBonus["damagetype"] != null)
								{
									strDamageType = "";
									strDamageExtra = objGear.WeaponBonus["damagetype"].InnerText;
								}
								// Adjust the Weapon's Damage.
								if (objGear.WeaponBonus["damage"] != null)
									strDamage += " + " + objGear.WeaponBonus["damage"].InnerText;
								if (objGear.WeaponBonus["damagereplace"] != null)
								{
									blnDamageReplaced = true;
									strDamage = objGear.WeaponBonus["damagereplace"].InnerText;
								}
							}
						}

						// Do the same for any plugins.
						foreach (Gear objChild in objGear.Gears)
						{
							if (objChild.WeaponBonus != null)
							{
								// Change the Weapon's Damage Type. (flechette rounds cannot affect weapons that have flechette included in their damage)
								if (!(objChild.WeaponBonus.InnerXml.Contains("(f)") && _strDamage.Contains("(f)")))
								{
									if (objChild.WeaponBonus["damagetype"] != null)
									{
										strDamageType = "";
										strDamageExtra = objChild.WeaponBonus["damagetype"].InnerText;
									}
									// Adjust the Weapon's Damage.
									if (objChild.WeaponBonus["damage"] != null)
										strDamage += " + " + objChild.WeaponBonus["damage"].InnerText;
									if (objChild.WeaponBonus["damagereplace"] != null)
									{
										blnDamageReplaced = true;
										strDamage = objChild.WeaponBonus["damagereplace"].InnerText;
									}
								}
								break;
							}
						}
					}
				}

				if (!blnDamageReplaced)
				{
					xprDamage = nav.Compile(strDamage);
					double dblDamage = 0;
					dblDamage = Math.Ceiling(Convert.ToDouble(nav.Evaluate(xprDamage), GlobalOptions.Instance.CultureInfo) + intBonus);
					strReturn = dblDamage.ToString() + strDamageType + strDamageExtra;
				}
				else
				{
					// Place the Damage Type (P or S) into a string and remove it from the expression.
					if (strDamage.Contains("P or S"))
					{
						strDamageType = "P or S";
						strDamage = strDamage.Replace("P or S", string.Empty);
					}
					else
					{
						if (strDamage.Contains("P"))
						{
							strDamageType = "P";
							strDamage = strDamage.Replace("P", string.Empty);
						}
						if (strDamage.Contains("S"))
						{
							strDamageType = "S";
							strDamage = strDamage.Replace("S", string.Empty);
						}
					}
					// Place any extra text like (e) and (f) in a string and remove it from the expression.
					if (strDamage.Contains("(e)"))
					{
						strDamageExtra = "(e)";
						strDamage = strDamage.Replace("(e)", string.Empty);
					}
					if (strDamage.Contains("(f)"))
					{
						strDamageExtra = "(f)";
						strDamage = strDamage.Replace("(f)", string.Empty);
					}
					// Replace the division sign with "div" since we're using XPath.
					strDamage = strDamage.Replace("/", " div ");

					xprDamage = nav.Compile(strDamage);
					double dblDamage = 0;
					dblDamage = Math.Ceiling(Convert.ToDouble(nav.Evaluate(xprDamage), GlobalOptions.Instance.CultureInfo) + intBonus);
					strReturn = dblDamage.ToString() + strDamageType + strDamageExtra;
				}
			}
			catch
			{
				strReturn = _strDamage;
			}

			// If the string couldn't be parsed (resulting in NaN which will happen if it is a special string like "Grenade", "Chemical", etc.), return the Weapon's Damage string.
			if (strReturn.StartsWith("NaN"))
				strReturn = _strDamage;

			// Translate the Damage Code.
			if (!blnForceEnglish)
			{
				strReturn = strReturn.Replace("S", LanguageManager.Instance.GetString("String_DamageStun"));
				strReturn = strReturn.Replace("P", LanguageManager.Instance.GetString("String_DamagePhysical"));

				strReturn = strReturn.Replace("Chemical", LanguageManager.Instance.GetString("String_DamageChemical"));
				strReturn = strReturn.Replace("Special", LanguageManager.Instance.GetString("String_DamageSpecial"));
				strReturn = strReturn.Replace("(e)", LanguageManager.Instance.GetString("String_DamageElectric"));
				strReturn = strReturn.Replace("(f)", LanguageManager.Instance.GetString("String_DamageFlechette"));
				strReturn = strReturn.Replace("P or S", LanguageManager.Instance.GetString("String_DamagePOrS"));
				strReturn = strReturn.Replace("Grenade", LanguageManager.Instance.GetString("String_DamageGrenade"));
				strReturn = strReturn.Replace("Missile", LanguageManager.Instance.GetString("String_DamageMissile"));
				strReturn = strReturn.Replace("Mortar", LanguageManager.Instance.GetString("String_DamageMortar"));
				strReturn = strReturn.Replace("Rocket", LanguageManager.Instance.GetString("String_DamageRocket"));
				strReturn = strReturn.Replace("Radius", LanguageManager.Instance.GetString("String_DamageRadius"));
				strReturn = strReturn.Replace("As Drug/Toxin", LanguageManager.Instance.GetString("String_DamageAsDrugToxin"));
				strReturn = strReturn.Replace("as round", LanguageManager.Instance.GetString("String_DamageAsRound"));
				strReturn = strReturn.Replace("/m", "/" + LanguageManager.Instance.GetString("String_DamageMeter"));
			}

			return strReturn;
		}

		/// <summary>
		/// Calculated Ammo capacity.
		/// </summary>
		public string CalculatedAmmo(bool blnForceEnglish = false)
		{
            string[] strSplit = new string[] { " " };
            string[] strAmmos = _strAmmo.Split(strSplit, StringSplitOptions.None);
            string strReturn = "";
            bool blnAdditionalClip = false;
            int intAmmoBonus = 0;

            foreach (WeaponMod objMod in WeaponMods)
            {
                // Replace the Ammo value.
                if (objMod.AmmoReplace != "")
                {
                    strAmmos = new string[] { objMod.AmmoReplace };
                    break;
                }

                // If the Mod is an Additional Clip that is not included with the base Weapon, turn on the Additional Clip flag.
                if (objMod.AdditionalClip && !objMod.IncludedInParent)
                    blnAdditionalClip = true;

                intAmmoBonus += objMod.AmmoBonus;
            }

            foreach (string strAmmo in strAmmos)
            {
                string strThisAmmo = strAmmo;
                if (strThisAmmo.Contains("("))
                {
                    int intAmmo = 0;
                    string strAmmoString = "";
                    string strPrepend = "";
                    try
                    {
                        strThisAmmo = strThisAmmo.Substring(0, strThisAmmo.IndexOf("("));
                        if (strThisAmmo.Contains("x"))
                        {
                            strPrepend = strThisAmmo.Substring(0, strThisAmmo.IndexOf("x") + 1);
                            strThisAmmo = strThisAmmo.Substring(strThisAmmo.IndexOf("x") + 1, strThisAmmo.Length - (strThisAmmo.IndexOf("x") + 1));
                        }

                        // If this is an Underbarrel Weapons that has been added, cut the Ammo capacity in half.
                        if (IsUnderbarrelWeapon && !IncludedInParent)
                            intAmmo = Convert.ToInt32(strThisAmmo) / 2;
                        else
                            intAmmo = Convert.ToInt32(strThisAmmo);

                        if (intAmmoBonus != 0)
                        {
                            double dblBonus = Convert.ToDouble(intAmmoBonus, GlobalOptions.Instance.CultureInfo) / 100.0;
                            intAmmo += Convert.ToInt32(Math.Ceiling(Convert.ToDouble(intAmmo, GlobalOptions.Instance.CultureInfo) * dblBonus));
                        }

                        strAmmoString = intAmmo.ToString();
                        if (strPrepend != "")
                            strAmmoString = strPrepend + strAmmoString;
                    }
                    catch
                    {
                    }
                    strThisAmmo = strAmmoString + strAmmo.Substring(strAmmo.IndexOf("("), strAmmo.Length - strAmmo.IndexOf("("));
                }
                strReturn += strThisAmmo + " ";
            }

            // If the Additional Clip flag is on, increment the clip multiplier.
            if (blnAdditionalClip)
            {
                if (strReturn.Contains("2x"))
                    strReturn = strReturn.Replace("2x", "3x");
                else
                    strReturn = "2x" + strReturn;
            }
            strReturn = strReturn.Trim();

            if (!blnForceEnglish)
            {
                // Translate the Ammo string.
                strReturn = strReturn.Replace(" or ", " " + LanguageManager.Instance.GetString("String_Or") + " ");
                strReturn = strReturn.Replace(" belt", LanguageManager.Instance.GetString("String_AmmoBelt"));
                strReturn = strReturn.Replace(" Energy", LanguageManager.Instance.GetString("String_AmmoEnergy"));
                strReturn = strReturn.Replace(" external source", LanguageManager.Instance.GetString("String_AmmoExternalSource"));
                strReturn = strReturn.Replace(" Special", LanguageManager.Instance.GetString("String_AmmoSpecial"));

                strReturn = strReturn.Replace("(b)", "(" + LanguageManager.Instance.GetString("String_AmmoBreakAction") + ")");
                strReturn = strReturn.Replace("(belt)", "(" + LanguageManager.Instance.GetString("String_AmmoBelt") + ")");
                strReturn = strReturn.Replace("(box)", "(" + LanguageManager.Instance.GetString("String_AmmoBox") + ")");
                strReturn = strReturn.Replace("(c)", "(" + LanguageManager.Instance.GetString("String_AmmoClip") + ")");
                strReturn = strReturn.Replace("(cy)", "(" + LanguageManager.Instance.GetString("String_AmmoCylinder") + ")");
                strReturn = strReturn.Replace("(d)", "(" + LanguageManager.Instance.GetString("String_AmmoDrum") + ")");
                strReturn = strReturn.Replace("(m)", "(" + LanguageManager.Instance.GetString("String_AmmoMagazine") + ")");
                strReturn = strReturn.Replace("(ml)", "(" + LanguageManager.Instance.GetString("String_AmmoMuzzleLoad") + ")");
            }

            return strReturn;
		}

        /// <summary>
        /// Total Accuracy for the Weapon.
        /// </summary>
        public new int TotalAccuracy
        {
            get
            {
                int intReturn = base.TotalAccuracy;
                ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
                if (_objCharacter != null)
                {
                    foreach (Improvement objImprovement in _objCharacter.Improvements)
                    {
                        if (objImprovement.ImproveType == Improvement.ImprovementType.WeaponSkillAccuracy && objImprovement.Enabled && (objImprovement.ImprovedName == ActiveSkill.Name))
                            intReturn += objImprovement.Value;
                    }
                }

                // Include any Accuracy adjustments from Weapon Accessories.
                foreach (WeaponAccessory objAccessory in WeaponAccessories)
                {
                    if (objAccessory.Equipped && !objAccessory.IncludedInParent)
                        intReturn += objAccessory.TotalAccuracy;
                }

                // Include any Accuracy adjustments from Weapon Mods.
                foreach (WeaponMod objMod in WeaponMods)
                {
                    if (objMod.Equipped && !objMod.IncludedInParent)
                        intReturn += objMod.TotalAccuracy;
                }

                intReturn += objImprovementManager.ValueOf(Improvement.ImprovementType.Accuracy);

                return intReturn;
            }
        }

		/// <summary>
		/// The Weapon's Firing Mode including Modifications.
		/// </summary>
		public string CalculatedMode
		{
			get
			{
				List<string> lstModes = new List<string>();
				string[] strModes = _strMode.Split('/');
				string strReturn = "";

				// Move the contents of the array to a list so it's easier to work with.
				foreach (string strMode in strModes)
					lstModes.Add(strMode);

				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.AddMode != "")
						lstModes.Add(objMod.AddMode);
				}

				if (lstModes.Contains("SS"))
					strReturn += LanguageManager.Instance.GetString("String_ModeSingleShot") + "/";
				if (lstModes.Contains("SA"))
					strReturn += LanguageManager.Instance.GetString("String_ModeSemiAutomatic") + "/";
				if (lstModes.Contains("BF"))
					strReturn += LanguageManager.Instance.GetString("String_ModeBurstFire") + "/";
				if (lstModes.Contains("FA"))
					strReturn += LanguageManager.Instance.GetString("String_ModeFullAutomatic") + "/";
				if (lstModes.Contains("Special"))
					strReturn += LanguageManager.Instance.GetString("String_ModeSpecial") + "/";

				// Remove the trailing "/".
				if (strReturn != "")
					strReturn = strReturn.Substring(0, strReturn.Length - 1);

				return strReturn;
			}
		}

		/// <summary>
		/// Determine if the Weapon is capable of firing in a particular mode.
		/// </summary>
		/// <param name="strFindMode">Firing mode to find.</param>
		public bool AllowMode(string strFindMode)
		{
			string[] strModes = CalculatedMode.Split('/');
			foreach (string strMode in strModes)
			{
				if (strMode == strFindMode)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Weapon Cost to use when working with Total Cost price modifiers for Weapon Mods.
		/// </summary>
		public int MultipliableCost
		{
			get
			{
				int intWeaponCost = Convert.ToInt32(_strCost);
				int intReturn = intWeaponCost;

				// Run through the list of Weapon Mods.
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (!objMod.IncludedInParent)
					{
						if (!objMod.Cost.StartsWith("Total Cost"))
							intReturn += objMod.TotalCost;
					}
				}

				return intReturn;
			}
		}

		/// <summary>
		/// The Weapon's total cost including Accessories and Modifications.
		/// </summary>
		public int TotalCost
		{
			get
			{
				int intWeaponCost = Convert.ToInt32(_strCost);
				int intReturn = intWeaponCost;
				int intCostMultiplier = 1;
				int intWeaponCostMultiplier = 1;

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				// Run through the list of Weapon Mods.
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (!objMod.IncludedInParent)
					{
						intReturn += objMod.TotalCost;
					}
				}

				// Run through the Weapon Mods and see if anything changes the cost multiplier (Vintage mod).
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.AccessoryCostMultiplier > 1 || objMod.ModCostMultiplier > 1)
						intCostMultiplier = objMod.AccessoryCostMultiplier;
				}

				// Multiply the Total Cost of the Weapon (base cost + Mods only, not Accessories)
				intReturn *= intWeaponCostMultiplier;

				// Run through the Accessories and add in their cost. If the cost is "Weapon Cost", the Weapon's base cost is added in again.
				foreach (WeaponAccessory objAccessory in WeaponAccessories)
				{
					if (!objAccessory.IncludedInParent)
						intReturn += objAccessory.TotalCost;
				}

				// If this is a Cyberware or Gear Weapon, remove the Weapon Cost from this since it has already been paid for through the parent item (but is needed to calculate Mod price).
				if (_strCategory.StartsWith("Cyberware") || _strCategory == "Gear")
					intReturn -= intWeaponCost;

				// Include the cost of any Underbarrel Weapon.
                foreach (Weapon objUnderbarrel in Weapons)
                {
                    if (!objUnderbarrel.IncludedInParent)
                        intReturn += objUnderbarrel.TotalCost;
                }

				return intReturn;
			}
		}

		/// <summary>
		/// The cost of just the Weapon itself.
		/// </summary>
		public int OwnCost
		{
			get
			{
				int intWeaponCost = Convert.ToInt32(_strCost);
				int intReturn = intWeaponCost;
				int intCostMultiplier = 1;
				int intWeaponCostMultiplier = 1;

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				// Run through the Weapon Mods and see if anything changes the cost multiplier (Vintage mod).
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.AccessoryCostMultiplier > 1 || objMod.ModCostMultiplier > 1)
						intCostMultiplier = objMod.AccessoryCostMultiplier;
				}

				// Multiply the Total Cost of the Weapon (base cost + Mods only, not Accessories)
				intReturn *= intWeaponCostMultiplier;

				// If this is a Cyberware or Gear Weapon, remove the Weapon Cost from this since it has already been paid for through the parent item (but is needed to calculate Mod price).
				if (_strCategory.StartsWith("Cyberware") || _strCategory == "Gear")
					intReturn -= intWeaponCost;

				return intReturn;
			}
		}

		/// <summary>
		/// The Weapon's total AP including Ammunition.
		/// </summary>
		public string TotalAP
		{
			get
			{
				string strAP = _strAP;
				if (strAP == "-")
					strAP = "0";
				int intAP = 0;

				try
				{
					intAP = Convert.ToInt32(strAP);
				}
				catch
				{
					// If AP is not numeric (for example "-half"), do do anything and just return the weapon's AP.
					return _strAP.Replace("-half", LanguageManager.Instance.GetString("String_APHalf"));
				}

				bool blnAPReplaced = false;
				CommonFunctions objFunctions = new CommonFunctions(_objCharacter);

				// Check if the Weapon has Ammunition loaded and look for any Damage bonus/replacement.
				if (AmmoLoaded != "")
				{
					// Look for Ammo on the character.
					Gear objGear = (Gear)objFunctions.FindEquipment(AmmoLoaded, _objCharacter.Gear, typeof(Gear));
					if (objGear == null)
						objGear = (Gear)objFunctions.FindEquipment(AmmoLoaded, _objCharacter.Vehicles, typeof(Gear));
					if (objGear != null)
					{
						if (objGear.WeaponBonus != null)
						{
							// Change the Weapon's Damage Type. (flechette rounds cannot affect weapons that have flechette included in their damage)
							if (!(objGear.WeaponBonus.InnerXml.Contains("(f)") && _strDamage.Contains("(f)")))
							{
								// Armor-Piercing Flechettes (and any other that might come along that does not explicitly add +5 AP) should instead reduce
								// the AP for Flechette-only Weapons which have the standard Flechette +5 AP built into their stats.
								if (_strDamage.Contains("(f)") && objGear.Name.Contains("Flechette"))
								{
									intAP -= 5;
								}
								else
								{
									// Change the Weapon's Damage Type.
									if (objGear.WeaponBonus["apreplace"] != null)
									{
										blnAPReplaced = true;
										strAP = objGear.WeaponBonus["apreplace"].InnerText;
									}
									// Adjust the Weapon's Damage.
									if (objGear.WeaponBonus["ap"] != null)
										intAP += Convert.ToInt32(objGear.WeaponBonus["ap"].InnerText);
								}
							}
						}
					}

					if (_objCharacter != null)
					{
						// Add any UnarmedAP bonus for the Unarmed Attack item.
						if (ExternalId == Constants.UnarmedAttack)
						{
							foreach (Improvement objImprovement in _objCharacter.Improvements)
							{
								if (objImprovement.ImproveType == Improvement.ImprovementType.UnarmedAP && objImprovement.Enabled)
									intAP += objImprovement.Value;
							}
						}
					}
					// If this is an Unarmed Cyberware Weapon (belongs to the Cyberware category), add the Unarmed AP bonus an Adept may have.
					if (_strCategory == "Cyberware")
					{
						ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
						intAP += objImprovementManager.ValueOf(Improvement.ImprovementType.UnarmedAP);
					}
				}

				// Add any AP modifications from Weapon Mods.
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.Equipped)
						intAP += objMod.APBonus;
				}

				if (!blnAPReplaced)
				{
					if (intAP == 0)
						return "-";
					else if (intAP > 0)
						return "+" + intAP.ToString();
					else
						return intAP.ToString();
				}
				else
					return strAP.Replace("-half", LanguageManager.Instance.GetString("String_APHalf"));
			}
		}

		/// <summary>
		/// The Weapon's total RC including Accessories and Modifications.
		/// </summary>
		public string TotalRC
		{
			get
			{
				string strRCBase = "0";
				string strRCFull = "0";
				string strRC = "";
				int intRCBase = 0;
				int intRCFull = 0;
				int intRCModifier = 0;

				if (_strRC.Contains("("))
				{
					if (_strRC.Substring(0, 1) == "(")
					{
						// The string contains only RC from pieces that can be removed - "(x)" only.
						strRCFull = _strRC;
					}
					else
					{
						// The string contains a mix of both fixed and removable RC. "x(y)".
						int intPos = _strRC.IndexOf("(");
						strRCBase = _strRC.Substring(0, intPos);
						strRCFull = _strRC.Substring(intPos, _strRC.Length - intPos);
					}
				}
				else
				{
					// The string contains only RC from fixed pieces - "x" only.
					strRCBase = _strRC;
					strRCFull = _strRC;
				}

				intRCBase = Convert.ToInt32(strRCBase);
				intRCFull = Convert.ToInt32(strRCFull.Replace("(", string.Empty).Replace(")", string.Empty));

				if (intRCBase < 0)
				{
					intRCModifier = intRCBase;
					intRCBase = 0;
				}

				CommonFunctions objFunctions = new CommonFunctions(_objCharacter);

				// Check if the Weapon has Ammunition loaded and look for any Recoil bonus.
				if (AmmoLoaded != "")
				{
					Gear objGear = (Gear)objFunctions.FindEquipment(AmmoLoaded, _objCharacter.Gear, typeof(Gear));
					if (objGear == null)
						objGear = (Gear)objFunctions.FindEquipment(AmmoLoaded, _objCharacter.Vehicles, typeof(Gear));

					if (objGear != null)
					{
						if (objGear.WeaponBonus != null)
						{
							// Change the Weapon's Damage Type.
							if (objGear.WeaponBonus["rc"] != null)
							{
								intRCBase += Convert.ToInt32(objGear.WeaponBonus["rc"].InnerText);
								intRCFull += Convert.ToInt32(objGear.WeaponBonus["rc"].InnerText);
							}
						}
					}
				}

				// Now that we know the Weapon's RC values, run through all of the Accessories and add theirs to the mix.
				// Only add in the values for items that do not come with the weapon.
				foreach (WeaponAccessory objAccessory in WeaponAccessories)
				{
					if (objAccessory.RC != "" && objAccessory.Equipped)
					{
						if (objAccessory.RC.Contains("("))
						{
							intRCFull += Convert.ToInt32(objAccessory.RC.Replace("(", string.Empty).Replace(")", string.Empty));
						}
						else
						{
							intRCBase += Convert.ToInt32(objAccessory.RC);
							intRCFull += Convert.ToInt32(objAccessory.RC);
						}
					}
				}

				// Only add in the values for items that do not come with the weapon.
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.RC != "" && objMod.Equipped)
					{
						if (objMod.RC.Contains("("))
						{
							intRCFull += Convert.ToInt32(objMod.RC.Replace("(", string.Empty).Replace(")", string.Empty));
						}
						else
						{
							intRCBase += Convert.ToInt32(objMod.RC);
							intRCFull += Convert.ToInt32(objMod.RC);
						}
					}
				}

				// If the optional rule for a character's Strength affecting Recoil, factor that in.
				if (_objCharacter.Options.StrengthAffectsRecoil && _objCharacter.STR.TotalValue >= 6 && !_blnVehicleMounted)
				{
					if (_objCharacter.STR.TotalValue >= 18)
					{
						intRCBase += 4;
						intRCFull += 4;
					}
					else if (_objCharacter.STR.TotalValue >= 14 && _objCharacter.STR.TotalValue <= 17)
					{
						intRCBase += 3;
						intRCFull += 3;
					}
					else if (_objCharacter.STR.TotalValue >= 10 && _objCharacter.STR.TotalValue <= 13)
					{
						intRCBase += 2;
						intRCFull += 2;
					}
					else if (_objCharacter.STR.TotalValue >= 6 && _objCharacter.STR.TotalValue <= 9)
					{
						intRCBase += 1;
						intRCFull += 1;
					}
				}

				// If the full RC is not higher than the base, only the base value is shown.
				//if (intRCFull + intRCModifier <= intRCBase)
				//	strRC = intRCBase.ToString();
				//else
				strRC = intRCFull.ToString();

				return strRC;
			}
		}

		/// <summary>
		/// The full Reach of the Weapons including the Character's Reach.
		/// </summary>
		public int TotalReach
		{
			get
			{
				int intReach = _intReach;

				if (_strType == "Melee")
				{
					// Run through the Character's Improvements and add any Reach Improvements.
					foreach (Improvement objImprovement in _objCharacter.Improvements)
					{
						if (objImprovement.ImproveType == Improvement.ImprovementType.Reach && objImprovement.Enabled)
							intReach += Convert.ToInt32(objImprovement.Value);
					}
				}

				return intReach;
			}
		}

		/// <summary>
		/// The number of Weapon Mod slots remaining.
		/// </summary>
		public int SlotsRemaining
		{
			get
			{
				int intSlots = 6;

				foreach (WeaponMod objMod in WeaponMods)
				{
					// Only Mods that are installed and do not come included in the Weapon should consume slots.
					if (objMod.Equipped)
					{
						if (!objMod.IncludedInParent)
							intSlots -= objMod.Slots;
					}
				}

				// Each installed Underbarrel Weapon consumes 3 slots.
				if (Weapons.Count > 0)
				{
					foreach (Weapon objUnderbarrelWeapon in Weapons)
					{
						if (objUnderbarrelWeapon.Equipped && objUnderbarrelWeapon.Cost != 0)
							intSlots -= 3;
					}
				}

				return intSlots;
			}
		}

		/// <summary>
		/// Permanently alters the Weapon's Range category.
		/// </summary>
		/// <param name="strRange">name of the new Range category to use.</param>
		public void SetRange(string strRange)
		{
			_strRange = strRange;
		}

		/// <summary>
		/// Evalulate and return the requested Range for the Weapon.
		/// </summary>
		/// <param name="strFindRange">Range node to use.</param>
		private int Range(string strFindRange)
		{
			XmlDocument objXmlDocument = XmlManager.Instance.Load("ranges.xml");

			string strRangeCategory = _strCategory;
			if (_strRange != "")
				strRangeCategory = _strRange;

			string strRange = "";
			try
			{
				strRange = objXmlDocument.SelectSingleNode("/chummer/ranges/range[category = \"" + strRangeCategory + "\"]")[strFindRange].InnerText;
			}
			catch
			{
				return -1;
			}

			XPathNavigator nav = objXmlDocument.CreateNavigator();

			int intSTR = _objCharacter.STR.TotalValue;
			int intBOD = _objCharacter.BOD.TotalValue;

			// If this is a Throwing Weapon, include the ThrowRange bonuses in the character's STR.
			if (_strCategory == "Throwing Weapons" || _strUseSkill == "Throwing Weapons")
			{
				ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
				intSTR += objImprovementManager.ValueOf(Improvement.ImprovementType.ThrowRange);
			}

			strRange = strRange.Replace("STR", intSTR.ToString());
			strRange = strRange.Replace("BOD", intBOD.ToString());

			XPathExpression xprRange = nav.Compile(strRange);

			double dblReturn = Convert.ToDouble(nav.Evaluate(xprRange).ToString(), GlobalOptions.Instance.CultureInfo) * _dblRangeMultiplier;
			int intReturn = Convert.ToInt32(Math.Floor(dblReturn));

			return intReturn;
		}

		/// <summary>
		/// Weapon's total Range bonus from Modifications.
		/// </summary>
		public double RangeBonus
		{
			get
			{
				int intRangeBonus = 100;

				// Weapon Mods.
				foreach (WeaponMod objMod in WeaponMods)
					intRangeBonus += objMod.RangeBonus;

				// Check if the Weapon has Ammunition loaded and look for any Range bonus.
				if (AmmoLoaded != "")
				{
					CommonFunctions objFunctions = new CommonFunctions(_objCharacter);

					Gear objGear = (Gear)objFunctions.FindEquipment(AmmoLoaded, _objCharacter.Gear, typeof(Gear));
					if (objGear == null)
						objGear = (Gear)objFunctions.FindEquipment(AmmoLoaded, _objCharacter.Vehicles, typeof(Gear));

					if (objGear != null)
					{
						if (objGear.WeaponBonus != null)
						{
							intRangeBonus += objGear.WeaponBonusRange;
						}
					}
				}

				double dblRangeBonus = Convert.ToDouble(intRangeBonus, GlobalOptions.Instance.CultureInfo);
				dblRangeBonus /= 100;

				return dblRangeBonus;
			}
		}

		/// <summary>
		/// Weapon's Short Range.
		/// </summary>
		public string RangeShort
		{
			get
			{
				int intMin = Range("min");
				int intMax = Range("short");
				double dblRangeBonus = RangeBonus;

				double dblMin = Convert.ToDouble(intMin, GlobalOptions.Instance.CultureInfo) * dblRangeBonus;
				double dblMax = Convert.ToDouble(intMax, GlobalOptions.Instance.CultureInfo) * dblRangeBonus;
				intMin = Convert.ToInt32(Math.Ceiling(dblMin));
				intMax = Convert.ToInt32(Math.Ceiling(dblMax));

				if (intMin == -1 && intMax == -1)
					return "";
				else
					return intMin.ToString() + "-" + intMax.ToString();
			}
		}

		/// <summary>
		/// Weapon's Medium Range.
		/// </summary>
		public string RangeMedium
		{
			get
			{
				int intMin = Range("short");
				int intMax = Range("medium");
				double dblRangeBonus = RangeBonus;

				double dblMin = Convert.ToDouble(intMin, GlobalOptions.Instance.CultureInfo) * dblRangeBonus;
				double dblMax = Convert.ToDouble(intMax, GlobalOptions.Instance.CultureInfo) * dblRangeBonus;
				intMin = Convert.ToInt32(Math.Ceiling(dblMin));
				intMax = Convert.ToInt32(Math.Ceiling(dblMax));

				if (intMin == -1 && intMax == -1)
					return "";
				else
					return (intMin + 1).ToString() + "-" + intMax.ToString();
			}
		}

		/// <summary>
		/// Weapon's Long Range.
		/// </summary>
		public string RangeLong
		{
			get
			{
				int intMin = Range("medium");
				int intMax = Range("long");
				double dblRangeBonus = RangeBonus;

				double dblMin = Convert.ToDouble(intMin, GlobalOptions.Instance.CultureInfo) * dblRangeBonus;
				double dblMax = Convert.ToDouble(intMax, GlobalOptions.Instance.CultureInfo) * dblRangeBonus;
				intMin = Convert.ToInt32(Math.Ceiling(dblMin));
				intMax = Convert.ToInt32(Math.Ceiling(dblMax));

				if (intMin == -1 && intMax == -1)
					return "";
				else
					return (intMin + 1).ToString() + "-" + intMax.ToString();
			}
		}

		/// <summary>
		/// Weapon's Extreme Range.
		/// </summary>
		public string RangeExtreme
		{
			get
			{
				int intMin = Range("long");
				int intMax = Range("extreme");
				double dblRangeBonus = RangeBonus;

				double dblMin = Convert.ToDouble(intMin, GlobalOptions.Instance.CultureInfo) * dblRangeBonus;
				double dblMax = Convert.ToDouble(intMax, GlobalOptions.Instance.CultureInfo) * dblRangeBonus;
				intMin = Convert.ToInt32(Math.Ceiling(dblMin));
				intMax = Convert.ToInt32(Math.Ceiling(dblMax));

				if (intMin == -1 && intMax == -1)
					return "";
				else
					return (intMin + 1).ToString() + "-" + intMax.ToString();
			}
		}

		/// <summary>
		/// Number of rounds consumed by Full Burst.
		/// </summary>
		public int FullBurst
		{
			get
			{
				int intReturn = _intFullBurst;

				// Check to see if any of the Mods replace this value.
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.FullBurst > intReturn)
						intReturn = objMod.FullBurst;
				}

				return intReturn;
			}
		}

		/// <summary>
		/// Number of rounds consumed by Suppressive Fire.
		/// </summary>
		public int Suppressive
		{
			get
			{
				int intReturn = _intSuppressive;
				
				// Check to see if any of the Mods replace this value.
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.Suppressive > intReturn)
						intReturn = objMod.Suppressive;
				}

				return intReturn;
			}
		}

		/// <summary>
		/// Total Accessory Cost multiplier for the Weapon.
		/// </summary>
		public int AccessoryMultiplier
		{
			get
			{
				int intReturn = 0;
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.AccessoryCostMultiplier != 1)
						intReturn += objMod.AccessoryCostMultiplier;
				}

				if (intReturn == 0)
					intReturn = 1;

				return intReturn;
			}
		}

		/// <summary>
		/// Total Mod Cost multiplier for the Weapon.
		/// </summary>
		public int ModMultiplier
		{
			get
			{
				int intReturn = 0;
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.ModCostMultiplier != 1)
						intReturn += objMod.ModCostMultiplier;
					if (objMod.Cost.Contains("Total Cost"))
						intReturn += Convert.ToInt32(objMod.Cost.Replace("Total Cost * ", string.Empty));
				}

				if (intReturn == 0)
					intReturn = 1;

				return intReturn;
			}
		}

		/// <summary>
		/// The Dice Pool size for the Active Skill required to use the Weapon.
		/// </summary>
		public string DicePool
		{
			get
			{
				// Locate the Active Skill to be used.
                Skill objSkill = ActiveSkill;
                string strReturn = "";

				int intSmartlinkBonus = 0;
				int intDicePoolModifier = 0;
				// Determine if the Weapon has Smartgun.
				bool blnHasSmartgun = false;
				foreach (WeaponAccessory objAccessory in WeaponAccessories)
				{
					if (objAccessory.Smartgun && objAccessory.Equipped)
						blnHasSmartgun = true;

					if (objAccessory.Equipped)
						intDicePoolModifier += objAccessory.DicePool;
				}

				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.Smartgun && objMod.Equipped)
						blnHasSmartgun = true;

					// Include any Dice Pool modifier that comes from the Mod.
					if (objMod.Equipped)
						intDicePoolModifier += objMod.DicePool;
				}

				if (blnHasSmartgun)
				{
					if (objSkill.Name == "Automatics" || objSkill.Name == "Exotic Ranged Weapon" || objSkill.Name == "Heavy Weapons" || objSkill.Name == "Longarms" || objSkill.Name == "Pistols")
					{
						// Check to see if the character has the Smartlink Improvement.
						foreach (Improvement objImprovement in _objCharacter.Improvements)
						{
							if (objImprovement.ImproveType == Improvement.ImprovementType.Smartlink && objImprovement.Enabled)
								intSmartlinkBonus = 2;
						}
					}
				}

				foreach (Gear objGear in _objCharacter.Gear)
				{
					if (objGear.InternalId == AmmoLoaded)
					{
						if (objGear.WeaponBonus != null)
						{
							if (objGear.WeaponBonus["pool"] != null)
								intDicePoolModifier += Convert.ToInt32(objGear.WeaponBonus["pool"].InnerText);
						}
					}
				}

				int intRating = objSkill.TotalRating + intSmartlinkBonus + intDicePoolModifier;
				strReturn = intRating.ToString();

				// If the character has a Specialization, include it in the Dice Pool string.
				if (objSkill.Specialization != "" && !objSkill.ExoticSkill)
				{
					if (objSkill.Specialization == DisplayNameShort || objSkill.Specialization == _strName || objSkill.Specialization == DisplayCategory || objSkill.Specialization == _strCategory || (objSkill.Specialization != string.Empty && (objSkill.Specialization == _strSpec || objSkill.Specialization == _strSpec2)))
						strReturn += " (" + (intRating + 2).ToString() + ")";
				}

				return strReturn;
			}
		}

		/// <summary>
		/// Tooltip information for the Dice Pool.
		/// </summary>
		public string DicePoolTooltip
		{
			get
			{
				// Locate the Active Skill to be used.
                Skill objSkill = ActiveSkill;
                string strReturn = "";

				strReturn = objSkill.DisplayName + " (" + objSkill.TotalRating + ")";

				int intSmartlinkBonus = 0;
				// Determine if the Weapon has Smartgun.
				bool blnHasSmartgun = false;
				foreach (WeaponAccessory objAccessory in WeaponAccessories)
				{
					if (objAccessory.Smartgun && objAccessory.Equipped)
						blnHasSmartgun = true;

					if (objAccessory.Equipped && objAccessory.DicePool != 0)
						strReturn += " + " + objAccessory.DisplayNameShort + " (" + objAccessory.DicePool.ToString() + ")";
				}

				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.Smartgun && objMod.Equipped)
						blnHasSmartgun = true;

					// Include any Dice Pool modifier that comes from the Mod.
					if (objMod.Equipped && objMod.DicePool != 0)
						strReturn += " + " + objMod.DisplayNameShort + " (" + objMod.DicePool.ToString() + ")";
				}

				foreach (Gear objGear in _objCharacter.Gear)
				{
					if (objGear.InternalId == AmmoLoaded)
					{
						if (objGear.WeaponBonus != null)
						{
							if (objGear.WeaponBonus["pool"] != null)
								strReturn += " + " + objGear.DisplayNameShort + " (" + Convert.ToInt32(objGear.WeaponBonus["pool"].InnerText) + ")";
						}
					}
				}

				if (blnHasSmartgun)
				{
					if (objSkill.Name == "Automatics" || objSkill.Name == "Exotic Ranged Weapon" || objSkill.Name == "Heavy Weapons" || objSkill.Name == "Longarms" || objSkill.Name == "Pistols")
					{
						// Check to see if the character has the Smartlink Improvement.
						foreach (Improvement objImprovement in _objCharacter.Improvements)
						{
							if (objImprovement.ImproveType == Improvement.ImprovementType.Smartlink && objImprovement.Enabled)
								intSmartlinkBonus = 2;
						}
					}
				}

				if (objSkill.Specialization != "" && !objSkill.ExoticSkill)
				{
					if (objSkill.Specialization == DisplayNameShort || objSkill.Specialization == _strName || objSkill.Specialization == DisplayCategory || objSkill.Specialization == _strCategory || (objSkill.Specialization != string.Empty && (objSkill.Specialization == _strSpec || objSkill.Specialization == _strSpec2)))
						strReturn += " + " + LanguageManager.Instance.GetString("String_ExpenseSpecialization") + " (2)";
				}

				if (intSmartlinkBonus != 0)
					strReturn += " + " + LanguageManager.Instance.GetString("Tip_Skill_Smartlink") + " (" + intSmartlinkBonus + ")";

				return strReturn;
			}
		}

		// Run through the Weapon Mods and see if anything changes the cost multiplier (Vintage mod).
		public int CostMultiplier
		{
			get
			{
				int intReturn = 1;
				foreach (WeaponMod objMod in WeaponMods)
				{
					if (objMod.AccessoryCostMultiplier > 1 || objMod.ModCostMultiplier > 1)
						intReturn = objMod.AccessoryCostMultiplier;
				}

				return intReturn;
			}
		}

        public Skill ActiveSkill
        {
            get
            {
                string strCategory = _strCategory;
                string strSkill = "";
                string strSpec = "";

                // If this is a Special Weapon, use the Range to determine the required Active Skill (if present).
                if (strCategory == "Special Weapons" && _strRange != "")
                    strCategory = _strRange;

                // Exotic Skills require a matching Specialization.
                switch (strCategory)
                {
                    case "Bows":
                    case "Crossbows":
                        strSkill = "Archery";
                        break;
                    case "Assault Rifles":
                    case "Machine Pistols":
                    case "Submachine Guns":
                    case "Battle Rifles":
                        strSkill = "Automatics";
                        break;
                    case "Blades":
                    case "Cyberware Blades":
                        strSkill = "Blades";
                        break;
                    case "Clubs":
                        strSkill = "Clubs";
                        break;
                    case "Exotic Melee Weapons":
                    case "Cyberware Exotic Melee Weapons":
                        strSkill = "Exotic Melee Weapon";
                        strSpec = DisplayNameShort;
                        break;
                    case "Exotic Ranged Weapons":
                    case "Cyberware Exotic Ranged Weapons":
                    case "Cyberware Hold-Outs":
                    case "Cyberware Light Pistols":
                    case "Cyberware Machine Pistols":
                    case "Cyberware Heavy Pistols":
                    case "Cyberware Submachine Guns":
                    case "Cyberware Shotguns":
                    case "Cyberware Grenade Launchers":
                    case "Cyberware Tasers":
                        strSkill = "Exotic Ranged Weapon";
                        strSpec = DisplayNameShort;
                        break;
                    case "Flamethrowers":
                        strSkill = "Exotic Ranged Weapon";
                        strSpec = "Flamethrowers";
                        break;
                    case "Laser Weapons":
                        strSkill = "Exotic Ranged Weapon";
                        strSpec = "Laser Weapons";
                        break;
                    case "Assault Cannons":
                    case "Grenade Launchers":
                    case "Missile Launchers":
                    case "Mortar Launchers":
                    case "Light Machine Guns":
                    case "Medium Machine Guns":
                    case "Heavy Machine Guns":
                        strSkill = "Heavy Weapons";
                        break;
                    case "Shotguns":
                    case "Sniper Rifles":
                    case "Sports Rifles":
                        strSkill = "Longarms";
                        break;
                    case "Throwing Weapons":
                    case "Cyberware Throwing Weapons":
                        strSkill = "Throwing Weapons";
                        break;
                    case "Unarmed":
                    case "Cyberware Clubs":
                    case "Cyberware":
                        strSkill = "Unarmed Combat";
                        break;
                    default:
                        strSkill = "Pistols";
                        break;
                }

                // Use the Skill defined by the Weapon if one is present.
                if (_strUseSkill != string.Empty)
                {
                    strSkill = _strUseSkill;
                    strSpec = "";

                    if (_strUseSkill.Contains("Exotic"))
                        strSpec = DisplayNameShort;
                }

                // Locate the Active Skill to be used.
                Skill objSkill = new Skill(_objCharacter);
                foreach (Skill objCharacterSkill in _objCharacter.Skills)
                {
                    if (!objCharacterSkill.KnowledgeSkill && objCharacterSkill.Name == strSkill)
                    {
                        if (strSpec == "" || (strSpec != "" && objCharacterSkill.Specialization == strSpec))
                        {
                            objSkill = objCharacterSkill;
                            break;
                        }
                    }
                }

                return objSkill;
            }
        }
		#endregion
	}

	/// <summary>
	/// Weapon Accessory.
	/// </summary>
	public class WeaponAccessory : Equipment
	{
		private string _strMount = "";
		private string _strRC = "";
		private int _intConceal = 0;
		private XmlNode _nodAllowGear;
		private string _strDicePool = "";
        private bool _blnSmartgun = false;

		#region Constructor, Create, Save, Load, and Print Methods
		public WeaponAccessory(Character objCharacter) : base(objCharacter)
		{
		}

		/// Create a Weapon Accessory from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlAccessory">XmlNode to create the object from.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		/// <param name="strMount">Mount slot that the Weapon Accessory will consume.</param>
		public void Create(XmlNode objXmlAccessory, TreeNode objNode, string strMount)
		{
            _guidExternalID = Guid.Parse(objXmlAccessory["id"].InnerText);
			_strName = objXmlAccessory["name"].InnerText;
			_strMount = strMount;
			if (objXmlAccessory["rc"] != null)
				_strRC = objXmlAccessory["rc"].InnerText;
            if (objXmlAccessory["accuracy"] != null)
                _strAccuracy = objXmlAccessory["accuracy"].InnerText;
			if (objXmlAccessory["conceal"] != null)
				_intConceal = Convert.ToInt32(objXmlAccessory["conceal"].InnerText);
			_strAvail = objXmlAccessory["avail"].InnerText;
			_strSource = objXmlAccessory["source"].InnerText;
			_strPage = objXmlAccessory["page"].InnerText;
			_nodAllowGear = objXmlAccessory["allowgear"];
            if (objXmlAccessory["dicepool"] != null)
				_strDicePool = objXmlAccessory["dicepool"].InnerText;
            if (objXmlAccessory["smartgun"] != null)
                _blnSmartgun = Convert.ToBoolean(objXmlAccessory["smartgun"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");
				XmlNode objAccessoryNode = objXmlDocument.SelectSingleNode("/chummer/accessories/accessory[id = \"" + ExternalId + "\"]");
				if (objAccessoryNode != null)
				{
					if (objAccessoryNode["translate"] != null)
						_strAltName = objAccessoryNode["translate"].InnerText;
					if (objAccessoryNode["altpage"] != null)
						_strAltPage = objAccessoryNode["altpage"].InnerText;
				}
			}

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();

            // Check for a Variable Cost.
            if (objXmlAccessory["cost"].InnerText.StartsWith("Variable"))
                _strCost = VariableCost(objXmlAccessory["cost"].InnerText, DisplayNameShort).ToString();
            else
                _strCost = objXmlAccessory["cost"].InnerText;
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("accessory");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("mount", _strMount);
			objWriter.WriteElementString("rc", _strRC);
            objWriter.WriteElementString("accuracy", _strAccuracy);
			objWriter.WriteElementString("conceal", _intConceal.ToString());
			if (_strDicePool != "")
				objWriter.WriteElementString("dicepool", _strDicePool);
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("included", _blnIncludedInParent.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
            if (_blnSmartgun)
                objWriter.WriteElementString("smartgun", _blnSmartgun.ToString());
			if (_nodAllowGear != null)
				objWriter.WriteRaw(_nodAllowGear.OuterXml);
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			if (Gears.Count > 0)
			{
				objWriter.WriteStartElement("gears");
				foreach (Gear objGear in Gears)
				{
					// Use the Gear's SubClass if applicable.
					if (objGear.GetType() == typeof(Commlink))
					{
						Commlink objCommlink = new Commlink(_objCharacter);
						objCommlink = (Commlink)objGear;
						objCommlink.Save(objWriter);
					}
					else if (objGear.GetType() == typeof(OperatingSystem))
					{
						OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
						objOperatinSystem = (OperatingSystem)objGear;
						objOperatinSystem.Save(objWriter);
					}
					else
					{
						objGear.Save(objWriter);
					}
				}
				objWriter.WriteEndElement();
			}
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Attribute from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public override void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_strMount = objNode["mount"].InnerText;
			_strRC = objNode["rc"].InnerText;
            _strAccuracy = objNode["accuracy"].InnerText;
			_intConceal = Convert.ToInt32(objNode["conceal"].InnerText);
			_strAvail = objNode["avail"].InnerText;
			_strCost = objNode["cost"].InnerText;
			_blnIncludedInParent = Convert.ToBoolean(objNode["included"].InnerText);
			_blnEquipped = Convert.ToBoolean(objNode["equipped"].InnerText);
            if (objNode["smartgun"] != null)
                _blnSmartgun = Convert.ToBoolean(objNode["smartgun"].InnerText);

            try
			{
				_nodAllowGear = objNode["allowgear"];
			}
			catch
			{
			}
			
            _strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;

            try
			{
				_strDicePool = objNode["dicepool"].InnerText;
			}
			catch
			{
			}

			if (objNode["gears"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("gears/gear");
				foreach (XmlNode nodChild in nodChildren)
				{
					switch (nodChild["category"].InnerText)
					{
						case "Commlink":
						case "Commlink Upgrade":
							Commlink objCommlink = new Commlink(_objCharacter);
							objCommlink.Load(nodChild, blnCopy);
							objCommlink.Parent = this;
							Gears.Add(objCommlink);
							break;
						case "Commlink Operating System":
						case "Commlink Operating System Upgrade":
							OperatingSystem objOperatingSystem = new OperatingSystem(_objCharacter);
							objOperatingSystem.Load(nodChild, blnCopy);
							objOperatingSystem.Parent = this;
							Gears.Add(objOperatingSystem);
							break;
						default:
							Gear objGear = new Gear(_objCharacter);
							objGear.Load(nodChild, blnCopy);
							objGear.Parent = this;
							Gears.Add(objGear);
							break;
					}
				}
			}

			_strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");
				XmlNode objAccessoryNode = objXmlDocument.SelectSingleNode("/chummer/accessories/accessory[name = \"" + _strName + "\"]");
				if (objAccessoryNode != null)
				{
					if (objAccessoryNode["translate"] != null)
						_strAltName = objAccessoryNode["translate"].InnerText;
					if (objAccessoryNode["altpage"] != null)
						_strAltPage = objAccessoryNode["altpage"].InnerText;
				}
			}

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("accessory");
			objWriter.WriteElementString("name", DisplayName);
			objWriter.WriteElementString("mount", _strMount);
			objWriter.WriteElementString("rc", _strRC);
            objWriter.WriteElementString("accuracy", _strAccuracy);
			objWriter.WriteElementString("conceal", _intConceal.ToString());
			objWriter.WriteElementString("avail", TotalAvail);
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			objWriter.WriteElementString("included", _blnIncludedInParent.ToString());
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			if (Gears.Count > 0)
			{
				objWriter.WriteStartElement("gears");
				foreach (Gear objGear in Gears)
				{
					// Use the Gear's SubClass if applicable.
					if (objGear.GetType() == typeof(Commlink))
					{
						Commlink objCommlink = new Commlink(_objCharacter);
						objCommlink = (Commlink)objGear;
						objCommlink.Print(objWriter);
					}
					else if (objGear.GetType() == typeof(OperatingSystem))
					{
						OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
						objOperatinSystem = (OperatingSystem)objGear;
						objOperatinSystem.Print(objWriter);
					}
					else
					{
						objGear.Print(objWriter);
					}
				}
				objWriter.WriteEndElement();
			}
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Mount Used.
		/// </summary>
		public string Mount
		{
			get
			{
				return _strMount;
			}
			set
			{
				_strMount = value;
			}
		}

		/// <summary>
		/// Recoil.
		/// </summary>
		public string RC
		{
			get
			{
				return _strRC;
			}
			set
			{
				_strRC = value;
			}
		}

		/// <summary>
		/// Concealability.
		/// </summary>
		public int Concealability
		{
			get
			{
				return _intConceal;
			}
			set
			{
				_intConceal = value;
			}
		}

		/// <summary>
		/// Cost.
		/// </summary>
		public string Cost
		{
			get
			{
				// The Accessory has a cost of 0 if it is included in the base weapon configureation.
				if (_blnIncludedInParent)
					return "0";
				else
					return _strCost;
			}
			set
			{
				_strCost = value;
			}
		}

		/// <summary>
		/// AllowGear node from the XML file.
		/// </summary>
		public XmlNode AllowGear
		{
			get
			{
				return _nodAllowGear;
			}
			set
			{
				_nodAllowGear = value;
			}
		}

		/// <summary>
		/// Total cost of the Weapon Accessory.
		/// </summary>
		public int TotalCost
		{
			get
			{
				int intReturn = 0;

				if (_strCost == "Weapon Cost")
					intReturn = ((Weapon)Parent).Cost * ((Weapon)Parent).CostMultiplier;
				else
					intReturn = Convert.ToInt32(_strCost) * ((Weapon)Parent).CostMultiplier;

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				// Add in the cost of any Gear the Weapon Accessory has attached to it.
				foreach (Gear objGear in Gears)
					intReturn += objGear.TotalCost;

				return intReturn;
			}
		}

		/// <summary>
		/// The cost of just the Weapon Accessory itself.
		/// </summary>
		public int OwnCost
		{
			get
			{
				int intReturn = 0;

				if (_strCost == "Weapon Cost")
					intReturn = ((Weapon)Parent).Cost * ((Weapon)Parent).CostMultiplier;
				else
					intReturn = Convert.ToInt32(_strCost) * ((Weapon)Parent).CostMultiplier;

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				return intReturn;
			}
		}

		/// <summary>
		/// Dice Pool modifier.
		/// </summary>
		public int DicePool
		{
			get
			{
				int intReturn = 0;

				if (_strDicePool != string.Empty)
					intReturn = Convert.ToInt32(_strDicePool);

				return intReturn;
			}
		}

		private string DicePoolString
		{
			get
			{
				return _strDicePool;
			}
		}

        /// <summary>
        /// Whether or not the Weapon Accessory adds a Smartgun System to the Weapon.
        /// </summary>
        public bool Smartgun
        {
            get
            {
                return _blnSmartgun;
            }
            set
            {
                _blnSmartgun = value;
            }
        }
		#endregion
	}

	/// <summary>
	/// Weapon Modification.
	/// </summary>
	public class WeaponMod : Equipment
	{
		private int _intSlots = 0;
		private string _strRC = "";
		private int _intConceal = 0;
		private string _strAddMode = "";
		private int _intAmmoBonus = 0;
		private string _strAmmoReplace = "";
		private int _intDVBonus = 0;
		private int _intAPBonus = 0;
		private int _intRangeBonus = 0;
		private int _intAccessoryCostMultiplier = 1;
		private int _intModCostMultiplier = 1;
		private int _intFullBurst = 0;
		private int _intSuppressive = 0;
		private string _strDicePool = "";
        private bool _blnSmartgun = false;
        private bool _blnAdditionalClip = false;

		#region Constructor, Create, Save, Load, and Print Methods
		public WeaponMod(Character objCharacter) : base(objCharacter)
		{
		}

		/// Create a Weapon Modification from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlMod">XmlNode to create the object from.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		public void Create(XmlNode objXmlMod, TreeNode objNode)
		{
            _guidExternalID = Guid.Parse(objXmlMod["id"].InnerText);
			_strName = objXmlMod["name"].InnerText;
			_intSlots = Convert.ToInt32(objXmlMod["slots"].InnerText);
			_strAvail = objXmlMod["avail"].InnerText;
			if (objXmlMod["rc"] != null)
				_strRC = objXmlMod["rc"].InnerText;
            if (objXmlMod["accuracy"] != null)
                _strAccuracy = objXmlMod["accuracy"].InnerText;
			if (objXmlMod["conceal"] != null)
				_intConceal = Convert.ToInt32(objXmlMod["conceal"].InnerText);
			_strSource = objXmlMod["source"].InnerText;
			_strPage = objXmlMod["page"].InnerText;
            if (objXmlMod["ammobonus"] != null)
				_intAmmoBonus = Convert.ToInt32(objXmlMod["ammobonus"].InnerText);
			if (objXmlMod["ammoreplace"] != null)
				_strAmmoReplace = objXmlMod["ammoreplace"].InnerText;
            if (objXmlMod["accessorycostmultiplier"] != null)
				_intAccessoryCostMultiplier = Convert.ToInt32(objXmlMod["accessorycostmultiplier"].InnerText);
            if (objXmlMod["modcostmultiplier"] != null)
				_intModCostMultiplier = Convert.ToInt32(objXmlMod["modcostmultiplier"].InnerText);
            if (objXmlMod["addmode"] != null)
				_strAddMode = objXmlMod["addmode"].InnerText;
			if (objXmlMod["fullburst"] != null)
				_intFullBurst = Convert.ToInt32(objXmlMod["fullburst"].InnerText);
            if (objXmlMod["suppressive"] != null)
				_intSuppressive = Convert.ToInt32(objXmlMod["suppressive"].InnerText);
            if (objXmlMod["dvbonus"] != null)
				_intDVBonus = Convert.ToInt32(objXmlMod["dvbonus"].InnerText);
            if (objXmlMod["apbonus"] != null)
				_intAPBonus = Convert.ToInt32(objXmlMod["apbonus"].InnerText);
            if (objXmlMod["rangebonus"] != null)
				_intRangeBonus = Convert.ToInt32(objXmlMod["rangebonus"].InnerText);
            if (objXmlMod["dicepool"] != null)
				_strDicePool = objXmlMod["dicepool"].InnerText;
            if (objXmlMod["smartgun"] != null)
                _blnSmartgun = Convert.ToBoolean(objXmlMod["smartgun"].InnerText);
            if (objXmlMod["additionalclip"] != null)
                _blnAdditionalClip = Convert.ToBoolean(objXmlMod["additionalclip"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");
				XmlNode objModNode = objXmlDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + ExternalId + "\"]");
				if (objModNode != null)
				{
					if (objModNode["translate"] != null)
						_strAltName = objModNode["translate"].InnerText;
					if (objModNode["altpage"] != null)
						_strAltPage = objModNode["altpage"].InnerText;
				}
			}

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();

            // Check for a Variable Cost.
            if (objXmlMod["cost"].InnerText.StartsWith("Variable"))
                _strCost = VariableCost(objXmlMod["cost"].InnerText, DisplayNameShort).ToString();
            else
                _strCost = objXmlMod["cost"].InnerText;
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("weaponmod");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("slots", _intSlots.ToString());
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("rc", _strRC);
            objWriter.WriteElementString("accuracy", _strAccuracy);
			objWriter.WriteElementString("dvbonus", _intDVBonus.ToString());
			objWriter.WriteElementString("apbonus", _intAPBonus.ToString());
			objWriter.WriteElementString("conceal", _intConceal.ToString());
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("included", _blnIncludedInParent.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			if (_intAmmoBonus != 0)
				objWriter.WriteElementString("ammobonus", _intAmmoBonus.ToString());
			if (_strAmmoReplace != "")
				objWriter.WriteElementString("ammoreplace", _strAmmoReplace);
			if (_intAccessoryCostMultiplier != 1)
				objWriter.WriteElementString("accessorycostmultiplier", _intAccessoryCostMultiplier.ToString());
			if (_intModCostMultiplier != 1)
				objWriter.WriteElementString("modcostmultiplier", _intModCostMultiplier.ToString());
			if (_strAddMode != "")
				objWriter.WriteElementString("addmode", _strAddMode);
			if (_strDicePool != "")
				objWriter.WriteElementString("dicepool", _strDicePool);
			objWriter.WriteElementString("fullburst", _intFullBurst.ToString());
			objWriter.WriteElementString("suppressive", _intSuppressive.ToString());
			objWriter.WriteElementString("rangebonus", _intRangeBonus.ToString());
			objWriter.WriteElementString("extra", _strExtra);
            if (_blnSmartgun)
                objWriter.WriteElementString("smartgun", _blnSmartgun.ToString());
            if (_blnAdditionalClip)
                objWriter.WriteElementString("additionalclip", _blnAdditionalClip.ToString());
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Attribute from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public override void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_intSlots = Convert.ToInt32(objNode["slots"].InnerText);
			_strAvail = objNode["avail"].InnerText;
			_strRC = objNode["rc"].InnerText;
            _strAccuracy = objNode["accuracy"].InnerText;
			_intConceal = Convert.ToInt32(objNode["conceal"].InnerText);
			_strCost = objNode["cost"].InnerText;
			_blnIncludedInParent = Convert.ToBoolean(objNode["included"].InnerText);
			_blnEquipped = Convert.ToBoolean(objNode["equipped"].InnerText);
			_intRating = Convert.ToInt32(objNode["rating"].InnerText);
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;

            if (objNode["smartgun"] != null)
                _blnSmartgun = Convert.ToBoolean(objNode["smartgun"].InnerText);
            if (objNode["additionalclip"] != null)
                _blnAdditionalClip = Convert.ToBoolean(objNode["additionalclip"].InnerText);

            try
			{
				_intAmmoBonus = Convert.ToInt32(objNode["ammobonus"].InnerText);
			}
			catch
			{
			}
			try
			{
				_strAmmoReplace = objNode["ammoreplace"].InnerText;
			}
			catch
			{
			}
			try
			{
				_intAccessoryCostMultiplier = Convert.ToInt32(objNode["accessorycostmultiplier"].InnerText);
			}
			catch
			{
			}
			try
			{
				_intModCostMultiplier = Convert.ToInt32(objNode["modcostmultiplier"].InnerText);
			}
			catch
			{
			}
			try
			{
				_strAddMode = objNode["addmode"].InnerText;
			}
			catch
			{
			}
            try
            {
                _strDicePool = objNode["dicepool"].InnerText;
            }
            catch
            {
            }

			_strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);
			_intFullBurst = Convert.ToInt32(objNode["fullburst"].InnerText);
			_intSuppressive = Convert.ToInt32(objNode["suppressive"].InnerText);
			_intDVBonus = Convert.ToInt32(objNode["dvbonus"].InnerText);
			_intAPBonus = Convert.ToInt32(objNode["apbonus"].InnerText);
			_intRangeBonus = Convert.ToInt32(objNode["rangebonus"].InnerText);
			_strExtra = objNode["extra"].InnerText;

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");
				XmlNode objModNode = objXmlDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + ExternalId+ "\"]");
				if (objModNode != null)
				{
					if (objModNode["translate"] != null)
						_strAltName = objModNode["translate"].InnerText;
					if (objModNode["altpage"] != null)
						_strAltPage = objModNode["altpage"].InnerText;
				}
			}

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("weaponmod");
			objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("slots", _intSlots.ToString());
			objWriter.WriteElementString("avail", TotalAvail);
			objWriter.WriteElementString("rc", _strRC);
            objWriter.WriteElementString("accuracy", _strAccuracy);
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			objWriter.WriteElementString("included", _blnIncludedInParent.ToString());
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Slots used by the Mod.
		/// </summary>
		public int Slots
		{
			get
			{
				return _intSlots;
			}
			set
			{
				_intSlots = value;
			}
		}

		/// <summary>
		/// Concealability.
		/// </summary>
		public int Concealability
		{
			get
			{
				return _intConceal;
			}
			set
			{
				_intConceal = value;
			}
		}

		/// <summary>
		/// Recoil.
		/// </summary>
		public string RC
		{
			get
			{
				return _strRC;
			}
			set
			{
				_strRC = value;
			}
		}

		/// <summary>
		/// Cost.
		/// </summary>
		public string Cost
		{
			get
			{
				return _strCost;
			}
			set
			{
				_strCost = value;
			}
		}

		/// <summary>
		/// Adjust the Weapon's Ammo amount by the specified percent.
		/// </summary>
		public int AmmoBonus
		{
			get
			{
				return _intAmmoBonus;
			}
			set
			{
				_intAmmoBonus = value;
			}
		}

		/// <summary>
		/// Replace the Weapon's Ammo value with the Weapon Mod's value.
		/// </summary>
		public string AmmoReplace
		{
			get
			{
				return _strAmmoReplace;
			}
			set
			{
				_strAmmoReplace = value;
			}
		}

		/// <summary>
		/// Accessory cost multiplier (comes from the Vintage mod).
		/// </summary>
		public int AccessoryCostMultiplier
		{
			get
			{
				return _intAccessoryCostMultiplier;
			}
			set
			{
				_intAccessoryCostMultiplier = value;
			}
		}

		/// <summary>
		/// Mod cost multiplier (comes from the Vintage mod).
		/// </summary>
		public int ModCostMultiplier
		{
			get
			{
				return _intModCostMultiplier;
			}
			set
			{
				_intModCostMultiplier = value;
			}
		}

		/// <summary>
		/// Additional Weapon Firing Mode.
		/// </summary>
		public string AddMode
		{
			get
			{
				return _strAddMode;
			}
			set
			{
				_strAddMode = value;
			}
		}

		/// <summary>
		/// Number of rounds consumed by Full Burst.
		/// </summary>
		public int FullBurst
		{
			get
			{
				return _intFullBurst;
			}
		}

		/// <summary>
		/// Number of rounds consumed by Suppressive Fire.
		/// </summary>
		public int Suppressive
		{
			get
			{
				return _intSuppressive;
			}
		}

		/// <summary>
		/// Range bonus granted by the Modification.
		/// </summary>
		public int RangeBonus
		{
			get
			{
				return _intRangeBonus;
			}
		}

		/// <summary>
		/// DV bonus granted by the Modification.
		/// </summary>
		public int DVBonus
		{
			get
			{
				return _intDVBonus;
			}
		}

		/// <summary>
		/// AP bonus granted by the Modification.
		/// </summary>
		public int APBonus
		{
			get
			{
				return _intAPBonus;
			}
		}

        /// <summary>
        /// Whether or not the Weapon Mod adds a Smartgun System to the Weapon.
        /// </summary>
        public bool Smartgun
        {
            get
            {
                return _blnSmartgun;
            }
            set
            {
                _blnSmartgun = value;
            }
        }

        /// <summary>
        /// Whether or not the Weapon Mod adds an Additional Clip to the Weapon.
        /// </summary>
        public bool AdditionalClip
        {
            get
            {
                return _blnAdditionalClip;
            }
            set
            {
                _blnAdditionalClip = value;
            }
        }
		#endregion

		#region Complex Properties
		/// <summary>
		/// Dice Pool modifier.
		/// </summary>
		public int DicePool
		{
			get
			{
				int intReturn = 0;

				if (_strDicePool != string.Empty)
				{
					if (_strDicePool == "Rating")
						intReturn = _intRating;
					else if (_strDicePool == "-Rating")
						intReturn = _intRating * -1;
					else
						intReturn = Convert.ToInt32(_strDicePool);
				}

				return intReturn;
			}
		}

		private string DicePoolString
		{
			get
			{
				return _strDicePool;
			}
		}

		/// <summary>
		/// Total cost of the Weapon Mod.
		/// </summary>
		public int TotalCost
		{
			get
			{
				int intReturn = 0;

				if (_strCost.StartsWith("Total Cost"))
				{
					int intMultiplier = Convert.ToInt32(_strCost.Replace("Total Cost * ", string.Empty));
					intReturn = ((Weapon)Parent).MultipliableCost * intMultiplier;
				}
				else
				{
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strCost = "";
					string strCostExpression = _strCost;

					strCost = strCostExpression.Replace("Weapon Cost", ((Weapon)Parent).Cost.ToString());
					strCost = strCost.Replace("Rating", _intRating.ToString());
					XPathExpression xprCost = nav.Compile(strCost);
					intReturn = (Convert.ToInt32(nav.Evaluate(xprCost).ToString()) * ((Weapon)Parent).CostMultiplier);
				}

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				return intReturn;
			}
		}

		/// <summary>
		/// The cost of just the Weapon Mod itself.
		/// </summary>
		public int OwnCost
		{
			get
			{
				return TotalCost;
			}
		}
		#endregion
	}

	/// <summary>
	/// Type of Lifestyle.
	/// </summary>
	public enum LifestyleType
	{
		Standard = 0,
		BoltHole = 1,
		Safehouse = 2,
		Advanced = 3,
	}

	/// <summary>
	/// Lifestyle.
	/// </summary>
	public class Lifestyle
	{
        private Guid _guidExternalID = new Guid();
		private Guid _guiID = new Guid();
		private string _strName = "";
		private int _intCost = 0;
		private int _intDice = 0;
		private int _intMultiplier = 0;
		private int _intMonths = 1;
		private int _intRoommates = 0;
		private int _intPercentage = 100;
		private string _strLifestyleName = "";
		private bool _blnPurchased = false;
		private string _strComforts = "";
		private string _strEntertainment = "";
		private string _strNecessities = "";
		private string _strNeighborhood = "";
		private string _strSecurity = "";
		private string _strSource = "";
		private string _strPage = "";
		private LifestyleType _objType = LifestyleType.Standard;
		private List<string> _lstQualities = new List<string>();
		private string _strNotes = "";

		private readonly Character _objCharacter;

		#region Helper Methods
		/// <summary>
		/// Convert a string to a LifestyleType.
		/// </summary>
		/// <param name="strValue">String value to convert.</param>
		public LifestyleType ConverToLifestyleType(string strValue)
		{
			switch (strValue)
			{
				case "BoltHole":
					return LifestyleType.BoltHole;
				case "Safehouse":
					return LifestyleType.Safehouse;
				case "Advanced":
					return LifestyleType.Advanced;
				default:
					return LifestyleType.Standard;
			}
		}
		#endregion

		#region Constructor, Create, Save, Load, and Print Methods
		public Lifestyle(Character objCharacter)
		{
			// Create the GUID for the new Lifestyle.
			_guiID = Guid.NewGuid();
			_objCharacter = objCharacter;
		}

		/// Create a Lifestyle from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlLifestyle">XmlNode to create the object from.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		public void Create(XmlNode objXmlLifestyle, TreeNode objNode)
		{
            _guidExternalID = Guid.Parse(objXmlLifestyle["id"].InnerText);
			_strName = objXmlLifestyle["name"].InnerText;
			_intCost = Convert.ToInt32(objXmlLifestyle["cost"].InnerText);
			_intDice = Convert.ToInt32(objXmlLifestyle["dice"].InnerText);
			_intMultiplier = Convert.ToInt32(objXmlLifestyle["multiplier"].InnerText);
			_strSource = objXmlLifestyle["source"].InnerText;
			_strPage = objXmlLifestyle["page"].InnerText;

			objNode.Text = DisplayName;
			objNode.Tag = _guiID;
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("lifestyle");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("cost", _intCost.ToString());
			objWriter.WriteElementString("dice", _intDice.ToString());
			objWriter.WriteElementString("multiplier", _intMultiplier.ToString());
			objWriter.WriteElementString("months", _intMonths.ToString());
			objWriter.WriteElementString("roommates", _intRoommates.ToString());
			objWriter.WriteElementString("percentage", _intPercentage.ToString());
			objWriter.WriteElementString("lifestylename", _strLifestyleName);
			objWriter.WriteElementString("purchased", _blnPurchased.ToString());
			objWriter.WriteElementString("comforts", _strComforts);
			objWriter.WriteElementString("entertainment", _strEntertainment);
			objWriter.WriteElementString("necessities", _strNecessities);
			objWriter.WriteElementString("neighborhood", _strNeighborhood);
			objWriter.WriteElementString("security", _strSecurity);
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("type", _objType.ToString());
			objWriter.WriteStartElement("qualities");
			foreach (string strQuality in _lstQualities)
				objWriter.WriteElementString("quality", strQuality);
			objWriter.WriteEndElement();
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Attribute from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_intCost = Convert.ToInt32(objNode["cost"].InnerText);
			_intDice = Convert.ToInt32(objNode["dice"].InnerText);
			_intMultiplier = Convert.ToInt32(objNode["multiplier"].InnerText);
			_intMonths = Convert.ToInt32(objNode["months"].InnerText);
			_intRoommates = Convert.ToInt32(objNode["roommates"].InnerText);
			_intPercentage = Convert.ToInt32(objNode["percentage"].InnerText);
			_strLifestyleName = objNode["lifestylename"].InnerText;
			_blnPurchased = Convert.ToBoolean(objNode["purchased"].InnerText);
			_strComforts = objNode["comforts"].InnerText;
			_strEntertainment = objNode["entertainment"].InnerText;
			_strNecessities = objNode["necessities"].InnerText;
			_strNeighborhood = objNode["neighborhood"].InnerText;
			_strSecurity = objNode["security"].InnerText;
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;

            try
			{
				foreach (XmlNode objXmlQuality in objNode.SelectNodes("qualities/quality"))
					_lstQualities.Add(objXmlQuality.InnerText);
			}
			catch
			{
			}

			_strNotes = objNode["notes"].InnerText;
			_objType = ConverToLifestyleType(objNode["type"].InnerText);

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
				_intMonths = 0;
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("lifestyle");
			objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("cost", _intCost.ToString());
			objWriter.WriteElementString("dice", _intDice.ToString());
			objWriter.WriteElementString("multiplier", _intMultiplier.ToString());
			objWriter.WriteElementString("months", _intMonths.ToString());
			objWriter.WriteElementString("purchased", _blnPurchased.ToString());
			objWriter.WriteElementString("lifestylename", _strLifestyleName);
			objWriter.WriteElementString("type", _objType.ToString());

			string strComforts = "";
			string strEntertainment = "";
			string strNecessities = "";
			string strNeighborhood = "";
			string strSecurity = "";

			// Retrieve the Advanced Lifestyle information if applicable.
			if (_strComforts != "")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("lifestyles.xml");

				XmlNode objXmlAspect = objXmlDocument.SelectSingleNode("/chummer/comforts/comfort[name = \"" + _strComforts + "\"]");
				if (objXmlAspect["translate"] != null)
					strComforts = objXmlAspect["translate"].InnerText;
				else
					strComforts = objXmlAspect["name"].InnerText;
				objXmlAspect = objXmlDocument.SelectSingleNode("/chummer/entertainments/entertainment[name = \"" + _strEntertainment + "\"]");
				if (objXmlAspect["translate"] != null)
					strEntertainment = objXmlAspect["translate"].InnerText;
				else
					strEntertainment = objXmlAspect["name"].InnerText;
				objXmlAspect = objXmlDocument.SelectSingleNode("/chummer/necessities/necessity[name = \"" + _strNecessities + "\"]");
				if (objXmlAspect["translate"] != null)
					strNecessities = objXmlAspect["translate"].InnerText;
				else
					strNecessities = objXmlAspect["name"].InnerText;
				objXmlAspect = objXmlDocument.SelectSingleNode("/chummer/neighborhoods/neighborhood[name = \"" + _strNeighborhood + "\"]");
				if (objXmlAspect["translate"] != null)
					strNeighborhood = objXmlAspect["translate"].InnerText;
				else
					strNeighborhood = objXmlAspect["name"].InnerText;
				objXmlAspect = objXmlDocument.SelectSingleNode("/chummer/securities/security[name = \"" + _strSecurity + "\"]");
				if (objXmlAspect["translate"] != null)
					strSecurity = objXmlAspect["translate"].InnerText;
				else
					strSecurity = objXmlAspect["name"].InnerText;
			}

			objWriter.WriteElementString("comforts", strComforts);
			objWriter.WriteElementString("entertainment", strEntertainment);
			objWriter.WriteElementString("necessities", strNecessities);
			objWriter.WriteElementString("neighborhood", strNeighborhood);
			objWriter.WriteElementString("security", strSecurity);

			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			objWriter.WriteStartElement("qualities");

			// Retrieve the Qualities for the Advanced Lifestyle if applicable.
			if (_lstQualities.Count > 0)
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("lifestyles.xml");
				XmlNode objNode;

				foreach (string strQuality in _lstQualities)
				{
					string strThisQuality = "";
					string strQualityName = strQuality.Substring(0, strQuality.IndexOf('[') - 1);
					objNode = objXmlDocument.SelectSingleNode("/chummer/qualities/quality[name = \"" + strQualityName + "\"]");
					if (objNode["translate"] != null)
						strThisQuality += objNode["translate"].InnerText;
					else
						strThisQuality += objNode["name"].InnerText;
					strThisQuality += " [" + objNode["lp"].InnerText + "LP]\n";

					objWriter.WriteElementString("quality", strThisQuality);
				}
			}
			objWriter.WriteEndElement();
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
        /// <summary>
        /// External identifier which identifies the item in the XML data file.
        /// </summary>
        public string ExternalId
        {
            get
            {
                return _guidExternalID.ToString();
            }
            set
            {
                _guidExternalID = Guid.Parse(value);
            }
        }

		/// <summary>
		/// Internal identifier which will be used to identify this Lifestyle in the Improvement system.
		/// </summary>
		public string InternalId
		{
			get
			{
				return _guiID.ToString();
			}
		}

		/// <summary>
		/// Name.
		/// </summary>
		public string Name
		{
			get
			{
				return _strName;
			}
			set
			{
				_strName = value;
			}
		}

		/// <summary>
		/// The name of the object as it should be displayed on printouts (translated name only).
		/// </summary>
		public string DisplayNameShort
		{
			get
			{
				string strReturn = _strName;
				// Get the translated name if applicable.
				if (GlobalOptions.Instance.Language != "en-us")
				{
					XmlDocument objXmlDocument = XmlManager.Instance.Load("lifestyles.xml");
					XmlNode objNode = objXmlDocument.SelectSingleNode("/chummer/lifestyles/lifestyle[name = \"" + _strName + "\"]");
					if (objNode != null)
					{
						if (objNode["translate"] != null)
							strReturn = objNode["translate"].InnerText;
					}
				}

				return strReturn;
			}
		}

		/// <summary>
		/// The name of the object as it should be displayed in lists. Name (Extra).
		/// </summary>
		public string DisplayName
		{
			get
			{
				string strReturn = DisplayNameShort;

				if (_strLifestyleName != "")
					strReturn += " (\"" + _strLifestyleName + "\")";

				return strReturn;
			}
		}

		/// <summary>
		/// Sourcebook.
		/// </summary>
		public string Source
		{
			get
			{
				return _strSource;
			}
			set
			{
				_strSource = value;
			}
		}

		/// <summary>
		/// Sourcebook Page Number.
		/// </summary>
		public string Page
		{
			get
			{
				string strReturn = _strPage;
				// Get the translated name if applicable.
				if (GlobalOptions.Instance.Language != "en-us")
				{
					XmlDocument objXmlDocument = XmlManager.Instance.Load("lifestyles.xml");
					XmlNode objNode = objXmlDocument.SelectSingleNode("/chummer/lifestyles/lifestyle[name = \"" + _strName + "\"]");
					if (objNode != null)
					{
						if (objNode["altpage"] != null)
							strReturn = objNode["altpage"].InnerText;
					}
				}

				return strReturn;
			}
			set
			{
				_strPage = value;
			}
		}

		/// <summary>
		/// Cost.
		/// </summary>
		public int Cost
		{
			get
			{
				return _intCost;
			}
			set
			{
				_intCost = value;
			}
		}

		/// <summary>
		/// Number of dice the character rolls to determine their statring Nuyen.
		/// </summary>
		public int Dice
		{
			get
			{
				return _intDice;
			}
			set
			{
				_intDice = value;
			}
		}

		/// <summary>
		/// Number the character multiplies the dice roll with to determine their starting Nuyen.
		/// </summary>
		public int Multiplier
		{
			get
			{
				return _intMultiplier;
			}
			set
			{
				_intMultiplier = value;
			}
		}

		/// <summary>
		/// Months purchased.
		/// </summary>
		public int Months
		{
			get
			{
				return _intMonths;
			}
			set
			{
				_intMonths = value;
			}
		}

		/// <summary>
		/// Whether or not the Lifestyle has been Purchased and no longer rented.
		/// </summary>
		public bool Purchased
		{
			get
			{
				return _blnPurchased;
			}
			set
			{
				_blnPurchased = value;
			}
		}

		/// <summary>
		/// Advance Lifestyle Comforts.
		/// </summary>
		public string Comforts
		{
			get
			{
				return _strComforts;
			}
			set
			{
				_strComforts = value;
			}
		}

		/// <summary>
		/// Advance Lifestyle Comforts.
		/// </summary>
		public string Entertainment
		{
			get
			{
				return _strEntertainment;
			}
			set
			{
				_strEntertainment = value;
			}
		}
		/// <summary>
		/// Advance Lifestyle Necessities.
		/// </summary>
		public string Necessities
		{
			get
			{
				return _strNecessities;
			}
			set
			{
				_strNecessities = value;
			}
		}

		/// <summary>
		/// Advance Lifestyle Neighborhood.
		/// </summary>
		public string Neighborhood
		{
			get
			{
				return _strNeighborhood;
			}
			set
			{
				_strNeighborhood = value;
			}
		}

		/// <summary>
		/// Advance Lifestyle Security.
		/// </summary>
		public string Security
		{
			get
			{
				return _strSecurity;
			}
			set
			{
				_strSecurity = value;
			}
		}

		/// <summary>
		/// Advanced Lifestyle Qualities.
		/// </summary>
		public List<string> Qualities
		{
			get
			{
				return _lstQualities;
			}
		}

		/// <summary>
		/// Notes.
		/// </summary>
		public string Notes
		{
			get
			{
				return _strNotes;
			}
			set
			{
				_strNotes = value;
			}
		}

		/// <summary>
		/// A custom name for the Lifestyle assigned by the player.
		/// </summary>
		public string LifestyleName
		{
			get
			{
				return _strLifestyleName;
			}
			set
			{
				_strLifestyleName = value;
			}
		}

		/// <summary>
		/// Type of the Lifestyle.
		/// </summary>
		public LifestyleType StyleType
		{
			get
			{
				return _objType;
			}
			set
			{
				_objType = value;
			}
		}

		/// <summary>
		/// Number of Roommates this Lifestyle is shared with.
		/// </summary>
		public int Roommates
		{
			get
			{
				return _intRoommates;
			}
			set
			{
				_intRoommates = value;
			}
		}

		/// <summary>
		/// Percentage of the total cost the character pays per month.
		/// </summary>
		public int Percentage
		{
			get
			{
				return _intPercentage;
			}
			set
			{
				_intPercentage = value;
			}
		}
		#endregion

		#region Complex Properties
		/// <summary>
		/// Total cost of the Lifestyle.
		/// </summary>
		public int TotalCost
		{
			get
			{
				return TotalMonthlyCost * _intMonths;
			}
		}

		/// <summary>
		/// Total monthly cost of the Lifestyle.
		/// </summary>
		public int TotalMonthlyCost
		{
			get
			{
				ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
				double dblMultiplier = 1.0;
				double dblModifier = Convert.ToDouble(objImprovementManager.ValueOf(Improvement.ImprovementType.LifestyleCost), GlobalOptions.Instance.CultureInfo);
				if (_objType == LifestyleType.Standard)
					dblModifier += Convert.ToDouble(objImprovementManager.ValueOf(Improvement.ImprovementType.BasicLifestyleCost), GlobalOptions.Instance.CultureInfo);
				double dblRoommates = 1.0 + (0.1 * _intRoommates);
				dblMultiplier = 1.0 + Convert.ToDouble(dblModifier / 100, GlobalOptions.Instance.CultureInfo);
				double dblPercentage = Convert.ToDouble(_intPercentage, GlobalOptions.Instance.CultureInfo) / 100.0;

				return Convert.ToInt32((Convert.ToDouble(_intCost, GlobalOptions.Instance.CultureInfo) * dblMultiplier) * dblRoommates * dblPercentage);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Set the InternalId for the Lifestyle. Used when editing an Advanced Lifestyle.
		/// </summary>
		/// <param name="strInternalId">InternalId to set.</param>
		public void SetInternalId(string strInternalId)
		{
			_guiID = Guid.Parse(strInternalId);
		}
		#endregion
	}

    public enum AmmoType
    {
        None = 0,
        Ammo = 1,
        Minigrenade = 2,
        Missile = 3,
        Rocket = 4,
        Mortar = 5,
    }

	/// <summary>
	/// Standard Character Gear.
	/// </summary>
	public class Gear : Equipment
	{
		protected string _strCapacity = "";
		protected string _strArmorCapacity = "";
		protected string _strAvail3 = "";
		protected string _strAvail6 = "";
		protected string _strAvail10 = "";
		protected int _intCostFor = 1;
		protected int _intSignal = 0;
		protected string _strCost3 = "";
		protected string _strCost6 = "";
		protected string _strCost10 = "";
		protected bool _blnBonded = false;
		protected bool _blnHomeNode = false;
        protected bool _blnStackable = true;
		protected XmlNode _nodWeaponBonus;
		protected Guid _guiWeaponID = new Guid();
		protected string _strLocation = "";
		private int _intResponse = 0;
		private int _intFirewall = 0;
		private int _intSystem = 0;
		private int _intChildCostMultiplier = 1;
		private int _intChildAvailModifier = 0;
        private bool _blnErgonomic = false;
        private AmmoType _objAmmoType = AmmoType.None;

        #region Helper Methods
        /// <summary>
        /// Convert a string into an AmmoType.
        /// </summary>
        /// <param name="strAmmo">String to convert.</param>
        private AmmoType LoadAmmoType(string strAmmo)
        {
            AmmoType objReturn = AmmoType.None;
            switch (strAmmo)
            {
                case "Ammo":
                    objReturn = AmmoType.Ammo;
                    break;
                case "Minigrenade":
                    objReturn = AmmoType.Minigrenade;
                    break;
                case "Missile":
                    objReturn = AmmoType.Missile;
                    break;
                case "Rocket":
                    objReturn = AmmoType.Rocket;
                    break;
                case "Mortar":
                    objReturn = AmmoType.Mortar;
                    break;
                default:
                    objReturn = AmmoType.None;
                    break;
            }

            return objReturn;
        }

        /// <summary>
        /// Convert an AmmoType to a string.
        /// </summary>
        /// <param name="objAmmo">AmmoType to convert.</param>
        private string SaveAmmoType(AmmoType objAmmo)
        {
            string strReturn = "";
            strReturn = objAmmo.ToString();

            return strReturn;
        }
        #endregion

		#region Constructor, Create, Save, Load, and Print Methods
		public Gear(Character objCharacter) : base(objCharacter)
		{
		}
		
		/// Create a Gear from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlGear">XmlNode to create the object from.</param>
		/// <param name="objCharacter">Character the Gear is being added to.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		/// <param name="intRating">Selected Rating for the Gear.</param>
		/// <param name="objWeapons">List of Weapons that should be added to the character.</param>
		/// <param name="objWeaponNodes">List of TreeNodes to represent the added Weapons</param>
		/// <param name="strForceValue">Value to forcefully select for any ImprovementManager prompts.</param>
		/// <param name="blnHacked">Whether or not a Matrix Program has been hacked (removing the Copy Protection and Registration plugins).</param>
		/// <param name="blnInherent">Whether or not a Program is Inherent to an A.I.</param>
		/// <param name="blnAddImprovements">Whether or not Improvements should be added to the character.</param>
		/// <param name="blnCreateChildren">Whether or not child Gear should be created.</param>
		/// <param name="blnAerodynamic">Whether or not Weapons should be created as Aerodynamic.</param>
		public void Create(XmlNode objXmlGear, Character objCharacter, TreeNode objNode, int intRating, List<Weapon> objWeapons, List<TreeNode> objWeaponNodes, string strForceValue = "", bool blnHacked = false, bool blnInherent = false, bool blnAddImprovements = true, bool blnCreateChildren = true, bool blnAerodynamic = false)
		{
            _guidExternalID = Guid.Parse(objXmlGear["id"].InnerText);
			_strName = objXmlGear["name"].InnerText;
			_strCategory = objXmlGear["category"].InnerText;
			_strAvail = objXmlGear["avail"].InnerText;
            if (objXmlGear["accuracy"] != null)
                _strAccuracy = objXmlGear["accuracy"].InnerText;
			if (objXmlGear["avail3"] != null)
				_strAvail3 = objXmlGear["avail3"].InnerText;
            if (objXmlGear["avail6"] != null)
				_strAvail6 = objXmlGear["avail6"].InnerText;
            if (objXmlGear["avail10"] != null)
				_strAvail10 = objXmlGear["avail10"].InnerText;
            if (objXmlGear["capacity"] != null)
				_strCapacity = objXmlGear["capacity"].InnerText;
            if (objXmlGear["armorcapacity"] != null)
				_strArmorCapacity = objXmlGear["armorcapacity"].InnerText;
            if (objXmlGear["costfor"] != null)
            {
				_intCostFor = Convert.ToInt32(objXmlGear["costfor"].InnerText);
				_intQty = Convert.ToInt32(objXmlGear["costfor"].InnerText);
			}
            if (objXmlGear["cost"] != null)
				_strCost = objXmlGear["cost"].InnerText;
            if (objXmlGear["cost3"] != null)
				_strCost3 = objXmlGear["cost3"].InnerText;
            if (objXmlGear["cost6"] != null)
				_strCost6 = objXmlGear["cost6"].InnerText;
            if (objXmlGear["cost10"] != null)
				_strCost10 = objXmlGear["cost10"].InnerText;
			_nodBonus = objXmlGear["bonus"];
            _nodConditional = objXmlGear["conditional"];
			_intMaxRating = Convert.ToInt32(objXmlGear["rating"].InnerText);
            if (objXmlGear["minrating"] != null)
				_intMinRating = Convert.ToInt32(objXmlGear["minrating"].InnerText);
            if (objXmlGear["signal"] != null)
				_intSignal = Convert.ToInt32(objXmlGear["signal"].InnerText);
            if (objXmlGear["response"] != null)
				_intResponse = Convert.ToInt32(objXmlGear["response"].InnerText);
            if (objXmlGear["system"] != null)
				_intSystem = Convert.ToInt32(objXmlGear["system"].InnerText);
            if (objXmlGear["firewall"] != null)
				_intFirewall = Convert.ToInt32(objXmlGear["firewall"].InnerText);
            if (objXmlGear["ergonomic"] != null)
                _blnErgonomic = Convert.ToBoolean(objXmlGear["ergonomic"].InnerText);

            if (objXmlGear["ammotype"] != null)
                _objAmmoType = LoadAmmoType(objXmlGear["ammotype"].InnerText);

            _intRating = intRating;
			_strSource = objXmlGear["source"].InnerText;
			_strPage = objXmlGear["page"].InnerText;

			if (objXmlGear["childcostmultiplier"] != null)
				_intChildCostMultiplier = Convert.ToInt32(objXmlGear["childcostmultiplier"].InnerText);
            if (objXmlGear["childavailmodifier"] != null)
				_intChildAvailModifier = Convert.ToInt32(objXmlGear["childavailmodifier"].InnerText);
            if (objXmlGear["allowstacking"] != null)
                _blnStackable = Convert.ToBoolean(objXmlGear["allowstacking"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");
				XmlNode objGearNode = objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + ExternalId + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode["translate"] != null)
						_strAltName = objGearNode["translate"].InnerText;
					if (objGearNode["altpage"] != null)
						_strAltPage = objGearNode["altpage"].InnerText;
				}

				if (_strAltName.StartsWith("Stacked Focus"))
					_strAltName = _strAltName.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));

				objGearNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode.Attributes["translate"] != null)
						_strAltCategory = objGearNode.Attributes["translate"].InnerText;
				}

				if (_strAltCategory.StartsWith("Stacked Focus"))
					_strAltCategory = _strAltCategory.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));
			}

			// Check for a Variable Cost.
			if (objXmlGear["cost"] != null && blnAddImprovements)
			{
                if (objXmlGear["cost"].InnerText.StartsWith("Variable"))
                    _strCost = VariableCost(objXmlGear["cost"].InnerText, DisplayNameShort).ToString();
                else
                    _strCost = objXmlGear["cost"].InnerText;
			}

			string strSource = _guiID.ToString();

			objNode.Text = _strName;
			objNode.Tag = _guiID.ToString();

			// If the Gear is Ammunition, ask the user to select a Weapon Category for it to be limited to.
			if (_strCategory == "Ammunition" && _strName.StartsWith("Ammo:"))
			{
				frmSelectWeaponCategory frmPickWeaponCategory = new frmSelectWeaponCategory();
				frmPickWeaponCategory.Description = LanguageManager.Instance.GetString("String_SelectWeaponCategoryAmmo");
				if (strForceValue != "")
					frmPickWeaponCategory.OnlyCategory = strForceValue;
				frmPickWeaponCategory.ShowDialog();

				_strExtra = frmPickWeaponCategory.SelectedCategory;
				objNode.Text += " (" + _strExtra + ")";
			}

			// Add Gear Weapons if applicable.
			if (objXmlGear["addweapon"] != null)
			{
				XmlDocument objXmlWeaponDocument = XmlManager.Instance.Load("weapons.xml");

				// More than one Weapon can be added, so loop through all occurrences.
				foreach (XmlNode objXmlAddWeapon in objXmlGear.SelectNodes("addweapon"))
				{
					XmlNode objXmlWeapon = objXmlWeaponDocument.SelectSingleNode("/chummer/weapons/weapon[id = \"" + objXmlAddWeapon.InnerText + "\"]");

					TreeNode objGearWeaponNode = new TreeNode();
					Weapon objGearWeapon = new Weapon(objCharacter);
					objGearWeapon.Create(objXmlWeapon, objCharacter, objGearWeaponNode, null, null, null);
					objGearWeaponNode.ForeColor = SystemColors.GrayText;
					if (blnAerodynamic)
					{
						objGearWeapon.Name += " (" + LanguageManager.Instance.GetString("Checkbox_Aerodynamic") + ")";
						objGearWeapon.SetRange("Aerodynamic Grenades");
						objGearWeaponNode.Text = objGearWeapon.DisplayName;
						_strName += " (" + LanguageManager.Instance.GetString("Checkbox_Aerodynamic") + ")";
						objNode.Text = DisplayName;
					}
						
					objWeaponNodes.Add(objGearWeaponNode);
					objWeapons.Add(objGearWeapon);

					_guiWeaponID = Guid.Parse(objGearWeapon.InternalId);
				}
			}

			// If the item grants a bonus, pass the information to the Improvement Manager.
			if (objXmlGear["bonus"] != null)
			{
				// Do not apply the Improvements if this is a Focus, unless we're speicifically creating a Weapon Focus. This is to avoid creating the Foci's Improvements twice (once when it's first added
				// to the character which is incorrect, and once when the Focus is actually Bonded).
				bool blnApply = true;
				if ((_strCategory == "Foci" || _strCategory == "Metamagic Foci") && !objXmlGear["bonus"].InnerXml.Contains("selecttext"))
					blnApply = false;

				if (blnApply)
				{
					ImprovementManager objImprovementManager;
					if (blnAddImprovements)
						objImprovementManager = new ImprovementManager(objCharacter);
					else
						objImprovementManager = new ImprovementManager(null);

					objImprovementManager.ForcedValue = strForceValue;
					if (!objImprovementManager.CreateImprovements(Improvement.ImprovementSource.Gear, strSource, objXmlGear["bonus"], false, intRating, DisplayNameShort))
					{
						_guiID = Guid.Empty;
						return;
					}
					if (objImprovementManager.SelectedValue != "")
					{
						_strExtra = objImprovementManager.SelectedValue;
						objNode.Text += " (" + objImprovementManager.SelectedValue + ")";
					}
				}
			}

			// Check to see if there are any child elements.
			if (objXmlGear["gears"] != null && blnCreateChildren)
			{
				// Create Gear using whatever information we're given.
				foreach (XmlNode objXmlChild in objXmlGear.SelectNodes("gears/gear"))
				{
					Gear objChild = new Gear(_objCharacter);
					TreeNode objChildNode = new TreeNode();
					objChild.Name = objXmlChild["name"].InnerText;
					objChild.Category = objXmlChild["category"].InnerText;
					objChild.Avail = "0";
					objChild.Cost = "0";
					objChild.Source = _strSource;
					objChild.Page = _strPage;
					objChild.Parent = this;
					Gears.Add(objChild);

					objChildNode.Text = objChild.DisplayName;
					objChildNode.Tag = objChild.InternalId;
					objNode.Nodes.Add(objChildNode);
					objNode.Expand();
				}

				XmlDocument objXmlGearDocument = XmlManager.Instance.Load("gear.xml");
				CreateChildren(objXmlGearDocument, objXmlGear, this, objNode, objCharacter, blnHacked);
			}

			// Add the Copy Protection and Registration plugins to the Matrix program. This does not apply if Unwired is not enabled, Hacked is selected, or this is a Suite being added (individual programs will add it to themselves).
			if (blnCreateChildren)
			{
				if ((_strCategory == "Matrix Programs" || _strCategory == "Skillsofts" || _strCategory == "Autosofts" || _strCategory == "Autosofts, Agent" || _strCategory == "Autosofts, Drone") && objCharacter.Options.BookEnabled("UN") && !blnHacked && !_strName.StartsWith("Suite:"))
				{
					XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");

					if (_objCharacter.Options.AutomaticCopyProtection && !blnInherent)
					{
						Gear objPlugin1 = new Gear(_objCharacter);
						TreeNode objPlugin1Node = new TreeNode();
						objPlugin1.Create(objXmlDocument.SelectSingleNode("/chummer/gears/gear[name = \"Copy Protection\"]"), objCharacter, objPlugin1Node, _intRating, null, null);
						if (_intRating == 0)
							objPlugin1.Rating = 1;
						objPlugin1.Avail = "0";
						objPlugin1.Cost = "0";
						objPlugin1.Cost3 = "0";
						objPlugin1.Cost6 = "0";
						objPlugin1.Cost10 = "0";
						objPlugin1.Capacity = "[0]";
						objPlugin1.Parent = this;
						Gears.Add(objPlugin1);
						objNode.Nodes.Add(objPlugin1Node);
					}

					if (_objCharacter.Options.AutomaticRegistration && !blnInherent)
					{
						Gear objPlugin2 = new Gear(_objCharacter);
						TreeNode objPlugin2Node = new TreeNode();
						objPlugin2.Create(objXmlDocument.SelectSingleNode("/chummer/gears/gear[name = \"Registration\"]"), objCharacter, objPlugin2Node, 0, null, null);
						objPlugin2.Avail = "0";
						objPlugin2.Cost = "0";
						objPlugin2.Cost3 = "0";
						objPlugin2.Cost6 = "0";
						objPlugin2.Cost10 = "0";
						objPlugin2.Capacity = "[0]";
						objPlugin2.Parent = this;
						Gears.Add(objPlugin2);
						objNode.Nodes.Add(objPlugin2Node);
						objNode.Expand();
					}

					if ((objCharacter.Metatype == "A.I." || objCharacter.MetatypeCategory == "Technocritters" || objCharacter.MetatypeCategory == "Protosapients") && blnInherent)
					{
						Gear objPlugin3 = new Gear(_objCharacter);
						TreeNode objPlugin3Node = new TreeNode();
						objPlugin3.Create(objXmlDocument.SelectSingleNode("/chummer/gears/gear[name = \"Ergonomic\"]"), objCharacter, objPlugin3Node, 0, null, null);
						objPlugin3.Avail = "0";
						objPlugin3.Cost = "0";
						objPlugin3.Cost3 = "0";
						objPlugin3.Cost6 = "0";
						objPlugin3.Cost10 = "0";
						objPlugin3.Capacity = "[0]";
						objPlugin3.Parent = this;
						Gears.Add(objPlugin3);
						objNode.Nodes.Add(objPlugin3Node);

						Gear objPlugin4 = new Gear(_objCharacter);
						TreeNode objPlugin4Node = new TreeNode();
						objPlugin4.Create(objXmlDocument.SelectSingleNode("/chummer/gears/gear[name = \"Optimization\" and category = \"Program Options\"]"), objCharacter, objPlugin4Node, _intRating, null, null);
						if (_intRating == 0)
							objPlugin4.Rating = 1;
						objPlugin4.Avail = "0";
						objPlugin4.Cost = "0";
						objPlugin4.Cost3 = "0";
						objPlugin4.Cost6 = "0";
						objPlugin4.Cost10 = "0";
						objPlugin4.Capacity = "[0]";
						objPlugin4.Parent = this;
						Gears.Add(objPlugin4);
						objNode.Nodes.Add(objPlugin4Node);
						objNode.Expand();
					}
				}
			}

			// If the item grants a Weapon bonus (Ammunition), just fill the WeaponBonus XmlNode.
			if (objXmlGear["weaponbonus"] != null)
				_nodWeaponBonus = objXmlGear["weaponbonus"];
			objNode.Text = DisplayName;
		}

		protected void CreateChildren(XmlDocument objXmlGearDocument, XmlNode objXmlGear, Gear objParent, TreeNode objNode, Character objCharacter, bool blnHacked)
		{
			// Create Gear by looking up the name of the item we're provided with.
			if (objXmlGear.SelectNodes("gears/usegear").Count > 0)
			{
				foreach (XmlNode objXmlChild in objXmlGear.SelectNodes("gears/usegear"))
				{
					XmlNode objXmlGearNode = objXmlGearDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + objXmlChild["id"].InnerText + "\"]");
					int intChildRating = 0;
					int intChildQty = 1;
					string strChildForceValue = "";
					if (objXmlChild["rating"] != null)
						intChildRating = Convert.ToInt32(objXmlChild["rating"].InnerText);
					if (objXmlChild["name"].Attributes["qty"] != null)
						intChildQty = Convert.ToInt32(objXmlChild["name"].Attributes["qty"].InnerText);
					if (objXmlChild["name"].Attributes["select"] != null)
						strChildForceValue = objXmlChild["name"].Attributes["select"].InnerText;

					Gear objChild = new Gear(_objCharacter);
					TreeNode objChildNode = new TreeNode();
					List<Weapon> lstChildWeapons = new List<Weapon>();
					List<TreeNode> lstChildWeaponNodes = new List<TreeNode>();
					objChild.Create(objXmlGearNode, objCharacter, objChildNode, intChildRating, lstChildWeapons, lstChildWeaponNodes, strChildForceValue, blnHacked);
					objChild.Quantity = intChildQty;
					objChild.Cost = "0";
					objChild.Cost3 = "0";
					objChild.Cost6 = "0";
					objChild.Cost10 = "0";
					objChild.MinRating = intChildRating;
					objChild.MaxRating = intChildRating;
					objChild.Parent = this;
					objParent.Gears.Add(objChild);

					// Change the Capacity of the child if necessary.
					if (objXmlChild["capacity"] != null)
						objChild.Capacity = "[" + objXmlChild["capacity"].InnerText + "]";

					objNode.Nodes.Add(objChildNode);
					objNode.Expand();

					CreateChildren(objXmlGearDocument, objXmlChild, objChild, objChildNode, objCharacter, blnHacked);
				}
			}
		}

		/// <summary>
		/// Copy a piece of Gear.
		/// </summary>
		/// <param name="objGear">Gear object to copy.</param>
		/// <param name="objNode">TreeNode for the copied item.</param>
		/// <param name="objWeapons">List of Weapons created by the copied item.</param>
		/// <param name="objWeaponNodes">List of TreeNodes for the Weapons created by the copied item.</param>
		public void Copy(Gear objGear, TreeNode objNode, List<Weapon> objWeapons, List<TreeNode> objWeaponNodes)
		{
			_strName = objGear.Name;
			_strCategory = objGear.Category;
			_intMaxRating = objGear.MaxRating;
			_intMinRating = objGear.MinRating;
			_intRating = objGear.Rating;
			_intQty = objGear.Quantity;
			_strCapacity = objGear.Capacity;
			_strArmorCapacity = objGear.ArmorCapacity;
			_strAvail = objGear.Avail;
			_strAvail3 = objGear.Avail3;
			_strAvail6 = objGear.Avail6;
			_strAvail10 = objGear.Avail10;
			_intCostFor = objGear.CostFor;
			_intSignal = objGear.Signal;
			_intResponse = objGear.Response;
			_intFirewall = objGear.Firewall;
			_intSystem = objGear.System;
			_strCost = objGear.Cost;
			_strCost3 = objGear.Cost3;
			_strCost6 = objGear.Cost6;
			_strCost10 = objGear.Cost10;
			_strSource = objGear.Source;
			_strPage = objGear.Page;
			_strExtra = objGear.Extra;
			_blnBonded = objGear.Bonded;
			_blnEquipped = objGear.Equipped;
			_blnHomeNode = objGear.HomeNode;
			_nodBonus = objGear.Bonus;
            _nodConditional = objGear.Conditional;
			_nodWeaponBonus = objGear.WeaponBonus;
			_guiWeaponID = Guid.Parse(objGear.WeaponID);
			_strNotes = objGear.Notes;
			_strLocation = objGear.Location;
			_intChildAvailModifier = objGear.ChildAvailModifier;
			_intChildCostMultiplier = objGear.ChildCostMultiplier;
			_strGivenName = objGear.GivenName;
            _blnErgonomic = objGear.IsErgonomic;

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();

			foreach (Gear objGearChild in objGear.Gears)
			{
				TreeNode objChildNode = new TreeNode();
				Gear objChild = new Gear(_objCharacter);
				if (objGearChild.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink.Copy(objGearChild, objChildNode, objWeapons, objWeaponNodes);
					objChild = objCommlink;
				}
				else if (objGearChild.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOS = new OperatingSystem(_objCharacter);
					objOS.Copy(objGearChild, objChildNode, objWeapons, objWeaponNodes);
					objChild = objOS;
				}
				else
					objChild.Copy(objGearChild, objChildNode, objWeapons, objWeaponNodes);
				Gears.Add(objChild);

				objNode.Nodes.Add(objChildNode);
				objNode.Expand();
			}
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("gear");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("category", _strCategory);
			objWriter.WriteElementString("capacity", _strCapacity);
			objWriter.WriteElementString("armorcapacity", _strArmorCapacity);
			objWriter.WriteElementString("minrating", _intMinRating.ToString());
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("qty", _intQty.ToString());
            objWriter.WriteElementString("allowstacking", _blnStackable.ToString());
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("avail3", _strAvail3);
			objWriter.WriteElementString("avail6", _strAvail6);
			objWriter.WriteElementString("avail10", _strAvail10);
			objWriter.WriteElementString("costfor", _intCostFor.ToString());
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("cost3", _strCost3);
			objWriter.WriteElementString("cost6", _strCost6);
			objWriter.WriteElementString("cost10", _strCost10);
			objWriter.WriteElementString("extra", _strExtra);
			objWriter.WriteElementString("bonded", _blnBonded.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("homenode", _blnHomeNode.ToString());
            if (_objAmmoType != AmmoType.None)
                objWriter.WriteElementString("ammotype", SaveAmmoType(_objAmmoType));
            if (_blnErgonomic)
                objWriter.WriteElementString("ergonomic", _blnErgonomic.ToString());
			if (_guiWeaponID != Guid.Empty)
				objWriter.WriteElementString("weaponguid", _guiWeaponID.ToString());
			if (_nodBonus != null)
				objWriter.WriteRaw("<bonus>" + _nodBonus.InnerXml + "</bonus>");
			else
				objWriter.WriteElementString("bonus", "");
            if (_nodConditional != null)
                objWriter.WriteRaw("<conditional>" + _nodConditional.InnerText + "</conditional>");
            else
                objWriter.WriteElementString("conditional", "");
			if (_nodWeaponBonus != null)
				objWriter.WriteRaw("<weaponbonus>" + _nodWeaponBonus.InnerXml + "</weaponbonus>");
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("response", _intResponse.ToString());
			objWriter.WriteElementString("firewall", _intFirewall.ToString());
			objWriter.WriteElementString("system", _intSystem.ToString());
			objWriter.WriteElementString("signal", _intSignal.ToString());
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("includedinparent", _blnIncludedInParent.ToString());
			if (_intChildCostMultiplier != 1)
				objWriter.WriteElementString("childcostmultiplier", _intChildCostMultiplier.ToString());
			if (_intChildAvailModifier != 0)
				objWriter.WriteElementString("childavailmodifier", _intChildAvailModifier.ToString());
			objWriter.WriteStartElement("children");
			foreach (Gear objGear in Gears)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink = (Commlink)objGear;
					objCommlink.Save(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Save(objWriter);
				}
				else
				{
					objGear.Save(objWriter);
				}
			}
			objWriter.WriteEndElement();
			objWriter.WriteElementString("location", _strLocation);
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Gear from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public override void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_strCategory = objNode["category"].InnerText;
			_strCapacity = objNode["capacity"].InnerText;
			_strArmorCapacity = objNode["armorcapacity"].InnerText;
			_intMinRating = Convert.ToInt32(objNode["minrating"].InnerText);
			_intMaxRating = Convert.ToInt32(objNode["maxrating"].InnerText);
			_intRating = Convert.ToInt32(objNode["rating"].InnerText);
			_intQty = Convert.ToInt32(objNode["qty"].InnerText);
			_strAvail = objNode["avail"].InnerText;
			_strAvail3 = objNode["avail3"].InnerText;
			_strAvail6 = objNode["avail6"].InnerText;
			_strAvail10 = objNode["avail10"].InnerText;
            _blnStackable = Convert.ToBoolean(objNode["allowstacking"].InnerText);
			_intCostFor = Convert.ToInt32(objNode["costfor"].InnerText);
			_strCost = objNode["cost"].InnerText;
			_strCost3 = objNode["cost3"].InnerText;
			_strCost6 = objNode["cost6"].InnerText;
			_strCost10 = objNode["cost10"].InnerText;
			if (objNode["extra"].InnerText == "Hold-Outs")
				objNode["extra"].InnerText = "Holdouts";
			_strExtra = objNode["extra"].InnerText;
			_blnBonded = Convert.ToBoolean(objNode["bonded"].InnerText);
			_blnEquipped = Convert.ToBoolean(objNode["equipped"].InnerText);
			_blnHomeNode = Convert.ToBoolean(objNode["homenode"].InnerText);
			_nodBonus = objNode["bonus"];
            _nodConditional = objNode["conditional"];
			try
			{
				_nodWeaponBonus = objNode["weaponbonus"];
			}
			catch
			{
			}
            if (objNode["ammotype"] != null)
                _objAmmoType = LoadAmmoType(objNode["ammotype"].InnerText);
            if (objNode["ergonomic"] != null)
                _blnErgonomic = Convert.ToBoolean(objNode["ergonomic"].InnerText);
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;
			_intSignal = Convert.ToInt32(objNode["signal"].InnerText);
			_intSystem = Convert.ToInt32(objNode["system"].InnerText);
			_intFirewall = Convert.ToInt32(objNode["firewall"].InnerText);
			_intResponse = Convert.ToInt32(objNode["response"].InnerText);

			if (objNode["weaponguid"] != null)
				_guiWeaponID = Guid.Parse(objNode["weaponguid"].InnerText);

			if (objNode["childcostmultiplier"] != null)
				_intChildCostMultiplier = Convert.ToInt32(objNode["childcostmultiplier"].InnerText);
			if (objNode["childavailmodifier"] != null)
				_intChildAvailModifier = Convert.ToInt32(objNode["childavailmodifier"].InnerText);

			_strGivenName = objNode["givenname"].InnerText;
			_blnIncludedInParent = Convert.ToBoolean(objNode["includedinparent"].InnerText);

			if (objNode["gear"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("children/gear");
				foreach (XmlNode nodChild in nodChildren)
				{
					switch (nodChild["category"].InnerText)
					{
						case "Commlink":
						case "Commlink Upgrade":
							Commlink objCommlink = new Commlink(_objCharacter);
							objCommlink.Load(nodChild, blnCopy);
							objCommlink.Parent = this;
							Gears.Add(objCommlink);
							break;
						case "Commlink Operating System":
						case "Commlink Operating System Upgrade":
							OperatingSystem objOperatingSystem = new OperatingSystem(_objCharacter);
							objOperatingSystem.Load(nodChild, blnCopy);
							objOperatingSystem.Parent = this;
							Gears.Add(objOperatingSystem);
							break;
						default:
							Gear objGear = new Gear(_objCharacter);
							objGear.Load(nodChild, blnCopy);
							objGear.Parent = this;
							Gears.Add(objGear);
							break;
					}
				}
			}

			_strLocation = objNode["location"].InnerText;
			_strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");
				XmlNode objGearNode = objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + ExternalId + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode["translate"] != null)
						_strAltName = objGearNode["translate"].InnerText;
					if (objGearNode["altpage"] != null)
						_strAltPage = objGearNode["altpage"].InnerText;
				}

				if (_strAltName.StartsWith("Stacked Focus"))
					_strAltName = _strAltName.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));

				objGearNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode.Attributes["translate"] != null)
						_strAltCategory = objGearNode.Attributes["translate"].InnerText;
				}

				if (_strAltCategory.StartsWith("Stacked Focus"))
					_strAltCategory = _strAltCategory.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));
			}

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
				_strLocation = string.Empty;
				_blnHomeNode = false;
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("gear");
			if ((_strCategory == "Foci" || _strCategory == "Metamagic Foci") && _blnBonded)
			{
				objWriter.WriteElementString("name", DisplayNameShort + " (" + LanguageManager.Instance.GetString("Label_BondedFoci") + ")");
			}
			else
				objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("name_english", _strName);
			objWriter.WriteElementString("category", DisplayCategory);
			objWriter.WriteElementString("category_english", _strCategory);
			objWriter.WriteElementString("iscommlink", false.ToString());
			objWriter.WriteElementString("ispersona", false.ToString());
			objWriter.WriteElementString("isnexus", (_strCategory == "Nexus").ToString());
			objWriter.WriteElementString("isammo", (_strCategory == "Ammunition").ToString());
			objWriter.WriteElementString("isprogram", IsProgram.ToString());
			objWriter.WriteElementString("isos", false.ToString());
			if (_strName == "Fake SIN")
				objWriter.WriteElementString("issin", true.ToString());
			else
				objWriter.WriteElementString("issin", false.ToString());
			objWriter.WriteElementString("capacity", _strCapacity);
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("qty", _intQty.ToString());
			objWriter.WriteElementString("avail", TotalAvail(true));
			objWriter.WriteElementString("avail_english", TotalAvail(true, true));
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			objWriter.WriteElementString("extra", LanguageManager.Instance.TranslateExtra(_strExtra));
			objWriter.WriteElementString("bonded", _blnBonded.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("homenode", _blnHomeNode.ToString());
			objWriter.WriteElementString("location", _strLocation);
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			objWriter.WriteStartElement("children");
			foreach (Gear objGear in Gears)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink = (Commlink)objGear;
					objCommlink.Print(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Print(objWriter);
				}
				else
				{
					objGear.Print(objWriter);
				}
			}
			objWriter.WriteEndElement();
			if (_nodWeaponBonus != null)
			{
				objWriter.WriteElementString("weaponbonusdamage", WeaponBonusDamage());
				objWriter.WriteElementString("weaponbonusdamage_english", WeaponBonusDamage(true));
				objWriter.WriteElementString("weaponbonusap", WeaponBonusAP);
			}
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Guid of a Cyberware Weapon.
		/// </summary>
		public string WeaponID
		{
			get
			{
				return _guiWeaponID.ToString();
			}
			set
			{
				_guiWeaponID = Guid.Parse(value);
			}
		}

		/// <summary>
		/// WeaponBonus node from the XML file.
		/// </summary>
		public XmlNode WeaponBonus
		{
			get
			{
				return _nodWeaponBonus;
			}
			set
			{
				_nodWeaponBonus = value;
			}
		}

		/// <summary>
		/// Gear capacity.
		/// </summary>
		public string Capacity
		{
			get
			{
				return _strCapacity;
			}
			set
			{
				_strCapacity = value;
			}
		}

		/// <summary>
		/// Armor capacity.
		/// </summary>
		public string ArmorCapacity
		{
			get
			{
				return _strArmorCapacity;
			}
			set
			{
				_strArmorCapacity = value;
			}
		}

		/// <summary>
		/// Availability for up to Rating 3.
		/// </summary>
		public string Avail3
		{
			get
			{
				return _strAvail3;
			}
			set
			{
				_strAvail3 = value;
			}
		}

		/// <summary>
		/// Availability for up to Rating 6.
		/// </summary>
		public string Avail6
		{
			get
			{
				return _strAvail6;
			}
			set
			{
				_strAvail6 = value;
			}
		}

		/// <summary>
		/// Availability for up to Rating 10.
		/// </summary>
		public string Avail10
		{
			get
			{
				return _strAvail10;
			}
			set
			{
				_strAvail10 = value;
			}
		}

		/// <summary>
		/// Use for ammo. The number of rounds that the nuyen amount buys.
		/// </summary>
		public int CostFor
		{
			get
			{
				return _intCostFor;
			}
			set
			{
				_intCostFor = value;
			}
		}

		/// <summary>
		/// Cost.
		/// </summary>
		public string Cost
		{
			get
			{
				return _strCost;
			}
			set
			{
				_strCost = value;
			}
		}

		/// <summary>
		/// Cost for up to Rating 3.
		/// </summary>
		public string Cost3
		{
			get
			{
				return _strCost3;
			}
			set
			{
				_strCost3 = value;
			}
		}

		/// <summary>
		/// Cost for up to Rating 6.
		/// </summary>
		public string Cost6
		{
			get
			{
				return _strCost6;
			}
			set
			{
				_strCost6 = value;
			}
		}

		/// <summary>
		/// Cost for up to Rating 10.
		/// </summary>
		public string Cost10
		{
			get
			{
				return _strCost10;
			}
			set
			{
				_strCost10 = value;
			}
		}

		/// <summary>
		/// Whether or not the Foci is bonded.
		/// </summary>
		public bool Bonded
		{
			get
			{
				return _blnBonded;
			}
			set
			{
				_blnBonded = value;
			}
		}

		/// <summary>
		/// Signal.
		/// </summary>
		public int Signal
		{
			get
			{
				int intSignal = _intSignal;
				foreach (Commlink objCommlink in Gears.OfType<Commlink>())
				{
					if (objCommlink.Category == "Commlink Upgrade")
					{
						if (objCommlink.Signal > intSignal)
							intSignal = objCommlink.Signal;
					}
				}

				return intSignal;
			}
			set
			{
				_intSignal = value;
			}
		}

		/// <summary>
		/// Response.
		/// </summary>
		public int Response
		{
			get
			{
				int intResponse = _intResponse;
				foreach (Commlink objCommlink in Gears.OfType<Commlink>())
				{
					if (objCommlink.Category == "Commlink Upgrade")
					{
						if (objCommlink.Response > intResponse)
							intResponse = objCommlink.Response;
					}
				}

				return intResponse;
			}
			set
			{
				_intResponse = value;
			}
		}

		/// <summary>
		/// System.
		/// </summary>
		public int System
		{
			get
			{
				int intSystem = _intSystem;
				foreach (OperatingSystem objOS in Gears.OfType<OperatingSystem>())
				{
					if (objOS.Category == "Commlink Operating System Upgrade")
					{
						if (objOS.System > intSystem)
							intSystem = objOS.System;
					}
				}

				return intSystem;
			}
			set
			{
				_intSystem = value;
			}
		}

		/// <summary>
		/// Firewall.
		/// </summary>
		public int Firewall
		{
			get
			{
				int intFirewall = _intFirewall;
				foreach (OperatingSystem objOS in Gears.OfType<OperatingSystem>())
				{
					if (objOS.Category == "Commlink Operating System Upgrade")
					{
						if (objOS.Firewall > intFirewall)
							intFirewall = objOS.Firewall;
					}
				}

				return intFirewall;
			}
			set
			{
				_intFirewall = value;
			}
		}

		/// <summary>
		/// Location.
		/// </summary>
		public string Location
		{
			get
			{
				return _strLocation;
			}
			set
			{
				_strLocation = value;
			}
		}

		/// <summary>
		/// Whether or not an item is an A.I.'s Home Node.
		/// </summary>
		public bool HomeNode
		{
			get
			{
				return _blnHomeNode;
			}
			set
			{
				_blnHomeNode = value;
			}
		}

		/// <summary>
		/// Whether or not the Gear qualifies as a Program in the printout XML.
		/// </summary>
		public bool IsProgram
		{
			get
			{
				if (_strCategory == "ARE Programs" || _strCategory.StartsWith("Autosofts") || _strCategory == "Data Software" || _strCategory == "Malware" || _strCategory == "Matrix Programs" || _strCategory == "Tactical AR Software" || _strCategory == "Telematics Infrastructure Software" || _strCategory == "Sensor Software")
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// Whether or not the Gear has the Ergonomic Program Option.
		/// </summary>
		public bool IsErgonomic
		{
			get
			{
				foreach (Gear objPlugin in Gears)
				{
					if (objPlugin.IsErgonomic)
						return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Cost multiplier for Children attached to this Gear.
		/// </summary>
		public int ChildCostMultiplier
		{
			get
			{
				return _intChildCostMultiplier;
			}
			set
			{
				_intChildCostMultiplier = value;
			}
		}

		/// <summary>
		/// Avail modifier for Children attached to this Gear.
		/// </summary>
		public int ChildAvailModifier
		{
			get
			{
				return _intChildAvailModifier;
			}
			set
			{
				_intChildAvailModifier = value;
			}
		}

        /// <summary>
        /// Whether or not the Gear can be combined into a stack.
        /// </summary>
        public bool IsStackable
        {
            get
            {
                return _blnStackable;
            }
            set
            {
                _blnStackable = value;
            }
        }

        /// <summary>
        /// AmmoType of the Gear if it is Ammunition.
        /// </summary>
        public AmmoType AmmoType
        {
            get
            {
                return _objAmmoType;
            }
            set
            {
                _objAmmoType = value;
            }
        }
		#endregion

		#region Complex Properties
		/// <summary>
		/// Total Availablility of the Gear and its accessories.
		/// </summary>
		public new string TotalAvail(bool blnCalculateAdditions = false, bool blnForceEnglish = false)
		{
			if (_strAvail == "")
				_strAvail = "0";

			bool blnIncludePlus = false;

			// If the Avail contains "+", return the base string and don't try to calculate anything since we're looking at a child component.
			if (_strAvail.StartsWith("+"))
			{
				blnIncludePlus = true;
				if (!blnCalculateAdditions)
					return _strAvail;
			}

			string strCalculated = "";
			string strReturn = "";

			if (_strAvail.Contains("Rating") || _strAvail3.Contains("Rating") || _strAvail6.Contains("Rating") || _strAvail10.Contains("Rating"))
			{
				// If the availability is determined by the Rating, evaluate the expression.
				XmlDocument objXmlDocument = new XmlDocument();
				XPathNavigator nav = objXmlDocument.CreateNavigator();

				string strAvail = "";
				string strAvailExpr = _strAvail;
				if (blnIncludePlus)
					strAvailExpr = strAvailExpr.Substring(1, strAvailExpr.Length - 1);

				if (_intRating <= 3 && _strAvail3 != "")
					strAvailExpr = _strAvail3;
				else if (_intRating <= 6 && _strAvail6 != "")
					strAvailExpr = _strAvail6;
				else if (_intRating >= 7 && _strAvail10 != "")
					strAvailExpr = _strAvail10;

				if (strAvailExpr.Substring(strAvailExpr.Length - 1, 1) == "F" || strAvailExpr.Substring(strAvailExpr.Length - 1, 1) == "R")
				{
					strAvail = strAvailExpr.Substring(strAvailExpr.Length - 1, 1);
					// Remove the trailing character if it is "F" or "R".
					strAvailExpr = strAvailExpr.Substring(0, strAvailExpr.Length - 1);
				}
				XPathExpression xprAvail = nav.Compile(strAvailExpr.Replace("Rating", _intRating.ToString()));
				strCalculated = Convert.ToInt32(nav.Evaluate(xprAvail)).ToString() + strAvail;
			}
			else
			{
				// Just a straight cost, so return the value.
				string strAvail = "";
				if (_strAvail.Contains("F") || _strAvail.Contains("R"))
				{
					strAvail = _strAvail.Substring(_strAvail.Length - 1, 1);
					strCalculated = Convert.ToInt32(_strAvail.Substring(0, _strAvail.Length - 1)) + strAvail;
				}
				else
					strCalculated = Convert.ToInt32(_strAvail).ToString();
			}

			int intAvail = 0;
			string strAvailText = "";
			if (strCalculated.Contains("F") || strCalculated.Contains("R"))
			{
				strAvailText = strCalculated.Substring(strCalculated.Length - 1);
				intAvail = Convert.ToInt32(strCalculated.Replace(strAvailText, string.Empty));
			}
			else
				intAvail = Convert.ToInt32(strCalculated);

			// Run through the child items and increase the Avail by any Mod whose Avail contains "+".
			foreach (Gear objChild in Gears)
			{
				if (objChild.Avail.StartsWith("+"))
				{
					if (objChild.Avail.Contains("R") || objChild.Avail.Contains("F"))
					{
						if (strAvailText != "F")
							strAvailText = objChild.Avail.Substring(objChild.Avail.Length - 1);
						intAvail += Convert.ToInt32(objChild.Avail.Replace("F", string.Empty).Replace("R", string.Empty));
					}
					else
						intAvail += Convert.ToInt32(objChild.Avail);
				}
			}

			// Add any Avail modifier that comes from its Parent.
			if (Parent != null)
			{
				if (Parent.GetType() == typeof(Gear))
					intAvail += ((Gear)Parent).ChildAvailModifier;
			}

			strReturn = intAvail.ToString() + strAvailText;

			// Translate the Avail string.
			if (!blnForceEnglish)
			{
				strReturn = strReturn.Replace("R", LanguageManager.Instance.GetString("String_AvailRestricted"));
				strReturn = strReturn.Replace("F", LanguageManager.Instance.GetString("String_AvailForbidden"));
			}

			if (blnIncludePlus)
				strReturn = "+" + strReturn;

			return strReturn;
		}

		/// <summary>
		/// Caculated Capacity of the Gear.
		/// </summary>
		public string CalculatedCapacity
		{
			get
			{
				if (_strCapacity.Contains("/["))
				{
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					int intPos = _strCapacity.IndexOf("/[");
					string strFirstHalf = _strCapacity.Substring(0, intPos);
					string strSecondHalf = _strCapacity.Substring(intPos + 1, _strCapacity.Length - intPos - 1);
					bool blnSquareBrackets = false;
					string strCapacity = "";

					try
					{
						blnSquareBrackets = strFirstHalf.Contains('[');
						strCapacity = strFirstHalf;
						if (blnSquareBrackets)
							strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
					}
					catch
					{
					}
					XPathExpression xprCapacity = nav.Compile(strCapacity.Replace("Rating", _intRating.ToString()));

					string strReturn = "";
					try
					{
						if (_strCapacity == "[*]")
							strReturn = "*";
						else
						{
							if (_strCapacity.StartsWith("FixedValues"))
                                strReturn = FixedValue(_strCapacity);
							else
								strReturn = nav.Evaluate(xprCapacity).ToString();
						}
						if (blnSquareBrackets)
							strReturn = "[" + strCapacity + "]";
					}
					catch
					{
						strReturn = "0";
					}
					strReturn += "/" + strSecondHalf;
					return strReturn;
				}
				else if (_strCapacity.Contains("Rating"))
				{
					// If the Capaicty is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					// XPathExpression cannot evaluate while there are square brackets, so remove them if necessary.
					bool blnSquareBrackets = _strCapacity.Contains('[');
					string strCapacity = _strCapacity;
					if (blnSquareBrackets)
						strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
					XPathExpression xprCapacity = nav.Compile(strCapacity.Replace("Rating", _intRating.ToString()));

					string strReturn = "";

					// This has resulted in a non-whole number, so round it (minimum of 1).
					decimal decNumber = Convert.ToDecimal(nav.Evaluate(xprCapacity), GlobalOptions.Instance.CultureInfo);
					int intNumber = Convert.ToInt32(Math.Floor(decNumber));
					if (intNumber < 1)
						intNumber = 1;
					strReturn = intNumber.ToString();

					if (blnSquareBrackets)
						strReturn = "[" + strReturn + "]";

					return strReturn;
				}
				else
				{
					// Just a straight Capacity, so return the value.
					if (_strCapacity == "")
						return "0";
					else
						return _strCapacity;
				}
			}
		}

		/// <summary>
		/// Caculated Armor Capacity of the Gear.
		/// </summary>
		public string CalculatedArmorCapacity
		{
			get
			{
				if (_strArmorCapacity.Contains("/["))
				{
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					int intPos = _strArmorCapacity.IndexOf("/[");
					string strFirstHalf = _strArmorCapacity.Substring(0, intPos);
					string strSecondHalf = _strArmorCapacity.Substring(intPos + 1, _strArmorCapacity.Length - intPos - 1);
					bool blnSquareBrackets = false;
					string strCapacity = "";

					try
					{
						blnSquareBrackets = strFirstHalf.Contains('[');
						strCapacity = strFirstHalf;
						if (blnSquareBrackets)
							strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
					}
					catch
					{
					}
					XPathExpression xprCapacity = nav.Compile(strCapacity.Replace("Rating", _intRating.ToString()));

					string strReturn = "";
					try
					{
						if (_strArmorCapacity == "[*]")
							strReturn = "*";
						else
						{
							if (_strArmorCapacity.StartsWith("FixedValues"))
                                strReturn = FixedValue(_strArmorCapacity);
							else
								strReturn = nav.Evaluate(xprCapacity).ToString();
						}
						if (blnSquareBrackets)
							strReturn = "[" + strCapacity + "]";
					}
					catch
					{
						strReturn = "0";
					}
					strReturn += "/" + strSecondHalf;
					return strReturn;
				}
				else if (_strArmorCapacity.Contains("Rating"))
				{
					// If the Capaicty is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					// XPathExpression cannot evaluate while there are square brackets, so remove them if necessary.
					bool blnSquareBrackets = _strArmorCapacity.Contains('[');
					string strCapacity = _strArmorCapacity;
					if (blnSquareBrackets)
						strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
					XPathExpression xprCapacity = nav.Compile(strCapacity.Replace("Rating", _intRating.ToString()));

					string strReturn = nav.Evaluate(xprCapacity).ToString();
					if (blnSquareBrackets)
						strReturn = "[" + strReturn + "]";

					return strReturn;
				}
				else
				{
					// Just a straight Capacity, so return the value.
					if (_strArmorCapacity == "")
						return "0";
					else
						return _strArmorCapacity;
				}
			}
		}

		/// <summary>
		/// Total cost of the just the Gear itself.
		/// </summary>
		public int CalculatedCost
		{
			get
			{
				int intReturn = 0;

				if (_strCost.Contains("Gear Cost") || _strCost3.Contains("Gear Cost") || _strCost6.Contains("Gear Cost") || _strCost10.Contains("Gear Cost"))
				{
					if (((Gear)Parent) != null)
					{
						string strCostExpression = "";

						if (_strCost != "")
							strCostExpression = _strCost;
						else
						{
							if (_intRating <= 3)
								strCostExpression = _strCost3;
							else if (_intRating <= 6)
								strCostExpression = _strCost6;
							else
								strCostExpression = _strCost10;
						}

						XmlDocument objXmlDocument = new XmlDocument();
						XPathNavigator nav = objXmlDocument.CreateNavigator();
						string strCost = "";
						strCost = strCostExpression.Replace("Gear Cost", ((Gear)Parent).CalculatedCost.ToString());
						XPathExpression xprCost = nav.Compile(strCost);
						intReturn = Convert.ToInt32(nav.Evaluate(xprCost).ToString());
					}
					else
						intReturn = 0;
				}
				else if (_strCost.Contains("Rating") || _strCost3.Contains("Rating") || _strCost6.Contains("Rating") || _strCost10.Contains("Rating"))
				{
					// If the cost is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strCost = "";
					string strCostExpression = "";

					if (_strCost != "")
						strCostExpression = _strCost;
					else
					{
						if (_intRating <= 3)
							strCostExpression = _strCost3;
						else if (_intRating <= 6)
							strCostExpression = _strCost6;
						else
							strCostExpression = _strCost10;
					}

					strCost = strCostExpression.Replace("Rating", _intRating.ToString());
					XPathExpression xprCost = nav.Compile(strCost);
					// This is first converted to a double and rounded up since some items have a multiplier that is not a whole number, such as 2.5.
					double dblCost = Math.Ceiling(Convert.ToDouble(nav.Evaluate(xprCost), GlobalOptions.Instance.CultureInfo));
					intReturn = Convert.ToInt32(dblCost);
				}
				else
				{
					// Just a straight cost, so return the value.
					intReturn = Convert.ToInt32(_strCost);
				}

				// The number is divided at the end for ammo purposes. This is done since the cost is per "costfor" but is being multiplied by the actual number of rounds.
				return (intReturn * _intQty) / _intCostFor;
			}
		}

		/// <summary>
		/// Total cost of the Gear and its accessories.
		/// </summary>
		public int TotalCost
		{
			get
			{
				int intReturn = 0;
				int intGearCost = 0;

				if (_strCost.Contains("Gear Cost") || _strCost3.Contains("Gear Cost") || _strCost6.Contains("Gear Cost") || _strCost10.Contains("Gear Cost"))
				{
					if (Parent != null)
					{
						string strCostExpression = "";

						if (_strCost != "")
							strCostExpression = _strCost;
						else
						{
							if (_intRating <= 3)
								strCostExpression = _strCost3;
							else if (_intRating <= 6)
								strCostExpression = _strCost6;
							else
								strCostExpression = _strCost10;
						}

						XmlDocument objXmlDocument = new XmlDocument();
						XPathNavigator nav = objXmlDocument.CreateNavigator();
						string strCost = "";
						strCost = strCostExpression.Replace("Gear Cost", ((Gear)Parent).CalculatedCost.ToString());
						XPathExpression xprCost = nav.Compile(strCost);
						intReturn = Convert.ToInt32(nav.Evaluate(xprCost).ToString());
					}
					else
						intReturn = 0;
				}
				else if (_strCost.Contains("Rating") || _strCost3.Contains("Rating") || _strCost6.Contains("Rating") || _strCost10.Contains("Rating") || _strCost.Contains("*") || _strCost3.Contains("*") || _strCost6.Contains("*") || _strCost10.Contains("*"))
				{
					// If the cost is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strCost = "";
					string strCostExpression = "";

					if (_strCost != "")
						strCostExpression = _strCost;
					else
					{
						if (_intRating <= 3)
							strCostExpression = _strCost3;
						else if (_intRating <= 6)
							strCostExpression = _strCost6;
						else
							strCostExpression = _strCost10;
					}

					strCost = strCostExpression.Replace("Rating", _intRating.ToString());
					XPathExpression xprCost = nav.Compile(strCost);
					// This is first converted to a double and rounded up since some items have a multiplier that is not a whole number, such as 2.5.
					double dblCost = Math.Ceiling(Convert.ToDouble(nav.Evaluate(xprCost), GlobalOptions.Instance.CultureInfo));
					intReturn = Convert.ToInt32(dblCost);
				}
				else
				{
					// Just a straight cost, so return the value.
					if (_strCost == string.Empty)
						intReturn = 0;
					else
						intReturn = Convert.ToInt32(_strCost);
				}
				intGearCost = intReturn;

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				// Add in the cost of all child components.
				int intPlugin = 0;
				foreach (Gear objChild in Gears)
					intPlugin += objChild.TotalCost;

				// The number is divided at the end for ammo purposes. This is done since the cost is per "costfor" but is being multiplied by the actual number of rounds.
				int intParentMultiplier = 1;
				if (Parent != null)
				{
					if (Parent.GetType() == typeof(Gear))
						intParentMultiplier = ((Gear)Parent).ChildCostMultiplier;
				}

				intReturn = (intReturn * _intQty * intParentMultiplier) / _intCostFor;
				// Add in the cost of the plugins separate since their value is not based on the Cost For number (it is always cost x qty).
				intReturn += intPlugin * _intQty;

				return intReturn;
			}
		}

		/// <summary>
		/// The cost of just the Gear itself.
		/// </summary>
		public int OwnCost
		{
			get
			{
				int intReturn = 0;
				int intGearCost = 0;

				if (_strCost.Contains("Gear Cost") || _strCost3.Contains("Gear Cost") || _strCost6.Contains("Gear Cost") || _strCost10.Contains("Gear Cost"))
				{
					if (Parent != null)
					{
						string strCostExpression = "";

						if (_strCost != "")
							strCostExpression = _strCost;
						else
						{
							if (_intRating <= 3)
								strCostExpression = _strCost3;
							else if (_intRating <= 6)
								strCostExpression = _strCost6;
							else
								strCostExpression = _strCost10;
						}

						XmlDocument objXmlDocument = new XmlDocument();
						XPathNavigator nav = objXmlDocument.CreateNavigator();
						string strCost = "";
						strCost = strCostExpression.Replace("Gear Cost", ((Gear)Parent).CalculatedCost.ToString());
						XPathExpression xprCost = nav.Compile(strCost);
						intReturn = Convert.ToInt32(nav.Evaluate(xprCost).ToString());
					}
					else
						intReturn = 0;
				}
				else if (_strCost.Contains("Rating") || _strCost3.Contains("Rating") || _strCost6.Contains("Rating") || _strCost10.Contains("Rating") || _strCost.Contains("*") || _strCost3.Contains("*") || _strCost6.Contains("*") || _strCost10.Contains("*"))
				{
					// If the cost is determined by the Rating, evaluate the expression.
					XmlDocument objXmlDocument = new XmlDocument();
					XPathNavigator nav = objXmlDocument.CreateNavigator();

					string strCost = "";
					string strCostExpression = "";

					if (_strCost != "")
						strCostExpression = _strCost;
					else
					{
						if (_intRating <= 3)
							strCostExpression = _strCost3;
						else if (_intRating <= 6)
							strCostExpression = _strCost6;
						else
							strCostExpression = _strCost10;
					}

					strCost = strCostExpression.Replace("Rating", _intRating.ToString());
					XPathExpression xprCost = nav.Compile(strCost);
					// This is first converted to a double and rounded up since some items have a multiplier that is not a whole number, such as 2.5.
					double dblCost = Math.Ceiling(Convert.ToDouble(nav.Evaluate(xprCost), GlobalOptions.Instance.CultureInfo));
					intReturn = Convert.ToInt32(dblCost);
				}
				else
				{
					// Just a straight cost, so return the value.
					if (_strCost == string.Empty)
						intReturn = 0;
					else
						intReturn = Convert.ToInt32(_strCost);
				}
				intGearCost = intReturn;

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				// The number is divided at the end for ammo purposes. This is done since the cost is per "costfor" but is being multiplied by the actual number of rounds.
				int intParentMultiplier = 1;
				if (Parent != null)
					intParentMultiplier = ((Gear)Parent).ChildCostMultiplier;

				intReturn = (intReturn * intParentMultiplier) / _intCostFor;

				return intReturn;
			}
		}

		/// <summary>
		/// The Gear's Capacity cost if used as a plugin.
		/// </summary>
		public int PluginCapacity
		{
			get
			{
				string strCapacity = CalculatedCapacity;
				if (strCapacity.Contains("/["))
				{
					// If this is a multiple-capacity item, use only the second half.
					int intPos = strCapacity.IndexOf("/[");
					strCapacity = strCapacity.Substring(intPos + 1);
				}

				// Only items that contain square brackets should consume Capacity. Everything else is treated as [0].
				if (strCapacity.Contains("["))
					strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
				else
					strCapacity = "0";
				return Convert.ToInt32(strCapacity);
			}
		}

		/// <summary>
		/// The Gear's Capacity cost if used as an Armor plugin.
		/// </summary>
		public int PluginArmorCapacity
		{
			get
			{
				string strCapacity = CalculatedArmorCapacity;
				if (strCapacity.Contains("/["))
				{
					// If this is a multiple-capacity item, use only the second half.
					int intPos = strCapacity.IndexOf("/[");
					strCapacity = strCapacity.Substring(intPos + 1);
				}

				// Only items that contain square brackets should consume Capacity. Everything else is treated as [0].
				if (strCapacity.Contains("["))
					strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
				else
					strCapacity = "0";
				return Convert.ToInt32(strCapacity);
			}
		}

		/// <summary>
		/// The amount of Capacity remaining in the Gear.
		/// </summary>
		public int CapacityRemaining
		{
			get
			{
				int intCapacity = 0;
				if (!_strCapacity.Contains("[") || _strCapacity.Contains("/["))
				{
					// Get the Gear base Capacity.
					if (_strCapacity.Contains("/["))
					{
						// If this is a multiple-capacity item, use only the first half.
						string strMyCapacity = CalculatedCapacity;
						int intPos = strMyCapacity.IndexOf("/[");
						strMyCapacity = strMyCapacity.Substring(0, intPos);
						intCapacity = Convert.ToInt32(strMyCapacity);
					}
					else
						intCapacity = Convert.ToInt32(CalculatedCapacity);

					// Run through its Children and deduct the Capacity costs.
					foreach (Gear objChildGear in Gears)
					{
						string strCapacity = objChildGear.CalculatedCapacity;
						if (strCapacity.Contains("/["))
						{
							// If this is a multiple-capacity item, use only the second half.
							int intPos = strCapacity.IndexOf("/[");
							strCapacity = strCapacity.Substring(intPos + 1);
						}

						// Only items that contain square brackets should consume Capacity. Everything else is treated as [0].
						if (strCapacity.Contains("["))
							strCapacity = strCapacity.Substring(1, strCapacity.Length - 2);
						else
							strCapacity = "0";
						intCapacity -= (Convert.ToInt32(strCapacity) * objChildGear.Quantity);
					}
				}

				return intCapacity;
			}
		}

		/// <summary>
		/// Weapon Bonus Damage.
		/// </summary>
		public string WeaponBonusDamage(bool blnForceEnglish = false)
		{
			if (_nodWeaponBonus == null)
				return "";
			else
			{
				string strReturn = "";
				// Use the damagereplace value if applicable.
				if (_nodWeaponBonus["damagereplace"] != null)
					strReturn = _nodWeaponBonus["damagereplace"].InnerText;
				else
				{
					// Use the damage bonus if available, otherwise use 0.
					if (_nodWeaponBonus["damage"] != null)
						strReturn = _nodWeaponBonus["damage"].InnerText;
					else
						strReturn = "0";

					// Attach the type if applicable.
					if (_nodWeaponBonus["damagetype"] != null)
						strReturn += _nodWeaponBonus["damagetype"].InnerText;

					// If this does not start with "-", add a "+" to the string.
					if (!strReturn.StartsWith("-"))
						strReturn = "+" + strReturn;
				}

				// Translate the Avail string.
				if (!blnForceEnglish)
				{
					strReturn = strReturn.Replace("P", LanguageManager.Instance.GetString("String_DamagePhysical"));
					strReturn = strReturn.Replace("S", LanguageManager.Instance.GetString("String_DamageStun"));
				}

				return strReturn;
			}
		}

		/// <summary>
		/// Weapon Bonus AP.
		/// </summary>
		public string WeaponBonusAP
		{
			get
			{
				if (_nodWeaponBonus == null)
					return "";
				else
				{
					string strReturn = "";
					// Use the apreplace value if applicable.
					if (_nodWeaponBonus["apreplace"] != null)
						strReturn = _nodWeaponBonus["apreplace"].InnerText;
					else
					{
						// Use the ap bonus if available, otherwise use 0.
						if (_nodWeaponBonus["ap"] != null)
							strReturn = _nodWeaponBonus["ap"].InnerText;
						else
							strReturn = "0";

						// If this does not start with "-", add a "+" to the string.
						if (!strReturn.StartsWith("-"))
							strReturn = "+" + strReturn;
					}

					return strReturn;
				}
			}
		}

		/// <summary>
		/// Weapon Bonus Range.
		/// </summary>
		public int WeaponBonusRange
		{
			get
			{
				if (_nodWeaponBonus == null)
					return 0;
				else
				{
					int intReturn = 0;

					if (_nodWeaponBonus["rangebonus"] != null)
						intReturn = Convert.ToInt32(_nodWeaponBonus["rangebonus"].InnerText);

					return intReturn;
				}
			}
		}
		#endregion
	}

	/// <summary>
	/// Commlink Device.
	/// </summary>
	public class Commlink : Gear
	{
		private int _intResponse = 1;
		private new int _intSignal = 1;
		private bool _blnIsLivingPersona = false;
		private bool _blnActiveCommlink = false;

		#region Constructor, Create, Save, Load, and Print Methods
		public Commlink(Character objCharacter) : base(objCharacter)
		{
		}

		/// Create a Commlink from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlGear">XmlNode to create the object from.</param>
		/// <param name="objCharacter">Character the Gear is being added to.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		/// <param name="intRating">Gear Rating.</param>
		/// <param name="blnAddImprovements">Whether or not Improvements should be added to the character.</param>
		/// <param name="blnCreateChildren">Whether or not child Gear should be created.</param>
		public void Create(XmlNode objXmlGear, Character objCharacter, TreeNode objNode, int intRating, bool blnAddImprovements = true, bool blnCreateChildren = true)
		{
            _guidExternalID = Guid.Parse(objXmlGear["id"].InnerText);
			_strName = objXmlGear["name"].InnerText;
			_strCategory = objXmlGear["category"].InnerText;
			_strAvail = objXmlGear["avail"].InnerText;
            if (objXmlGear["accuracy"] != null)
                _strAccuracy = objXmlGear["accuracy"].InnerText;
			if (objXmlGear["cost"] != null)
				_strCost = objXmlGear["cost"].InnerText;
			if (objXmlGear["cost3"] != null)
				_strCost3 = objXmlGear["cost3"].InnerText;
			if (objXmlGear["cost6"] != null)
				_strCost6 = objXmlGear["cost6"].InnerText;
            if (objXmlGear["cost10"] != null)
				_strCost10 = objXmlGear["cost10"].InnerText;
			if (objXmlGear["armorcapacity"] != null)
				_strArmorCapacity = objXmlGear["armorcapacity"].InnerText;
			_nodBonus = objXmlGear["bonus"];
            _nodConditional = objXmlGear["conditional"];
			_intMaxRating = Convert.ToInt32(objXmlGear["rating"].InnerText);
			_intRating = intRating;
			_strSource = objXmlGear["source"].InnerText;
			_strPage = objXmlGear["page"].InnerText;
			_intResponse = Convert.ToInt32(objXmlGear["response"].InnerText);
			_intSignal = Convert.ToInt32(objXmlGear["signal"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");
				XmlNode objGearNode = objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + ExternalId + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode["translate"] != null)
						_strAltName = objGearNode["translate"].InnerText;
					if (objGearNode["altpage"] != null)
						_strAltPage = objGearNode["altpage"].InnerText;
				}

				if (_strAltName.StartsWith("Stacked Focus"))
					_strAltName = _strAltName.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));

				objGearNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode.Attributes["translate"] != null)
						_strAltCategory = objGearNode.Attributes["translate"].InnerText;
				}

				if (_strAltCategory.StartsWith("Stacked Focus"))
					_strAltCategory = _strAltCategory.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));
			}

			string strSource = _guiID.ToString();

			objNode.Text = DisplayNameShort;
			objNode.Tag = _guiID.ToString();

			// If the item grants a bonus, pass the information to the Improvement Manager.
			if (objXmlGear["bonus"] != null)
			{
				ImprovementManager objImprovementManager;
				if (blnAddImprovements)
					objImprovementManager = new ImprovementManager(objCharacter);
				else
					objImprovementManager = new ImprovementManager(null);

				if (!objImprovementManager.CreateImprovements(Improvement.ImprovementSource.Gear, strSource, objXmlGear["bonus"], false, 1, DisplayNameShort))
				{
					_guiID = Guid.Empty;
					return;
				}
				if (objImprovementManager.SelectedValue != "")
				{
					_strExtra = objImprovementManager.SelectedValue;
					objNode.Text += " (" + objImprovementManager.SelectedValue + ")";
				}
			}

			// Check to see if there are any child elements.
			if (objXmlGear["gears"] != null && blnCreateChildren)
			{
				// Create Gear using whatever information we're given.
				foreach (XmlNode objXmlChild in objXmlGear.SelectNodes("gears/gear"))
				{
					Gear objChild = new Gear(_objCharacter);
					TreeNode objChildNode = new TreeNode();
					objChild.Name = objXmlChild["name"].InnerText;
					objChild.Category = objXmlChild["category"].InnerText;
					objChild.Avail = "0";
					objChild.Cost = "0";
					objChild.Source = _strSource;
					objChild.Page = _strPage;
					objChild.Parent = this;
					Gears.Add(objChild);

					objChildNode.Text = objChild.DisplayName;
					objChildNode.Tag = objChild.InternalId;
					objNode.Nodes.Add(objChildNode);
					objNode.Expand();
				}

				XmlDocument objXmlGearDocument = XmlManager.Instance.Load("gear.xml");
				CreateChildren(objXmlGearDocument, objXmlGear, this, objNode, objCharacter, blnCreateChildren);
			}

			// Add the Copy Protection and Registration plugins to the Matrix program. This does not apply if Unwired is not enabled, Hacked is selected, or this is a Suite being added (individual programs will add it to themselves).
			if (blnCreateChildren)
			{
				if ((_strCategory == "Matrix Programs" || _strCategory == "Skillsofts" || _strCategory == "Autosofts" || _strCategory == "Autosofts, Agent" || _strCategory == "Autosofts, Drone") && objCharacter.Options.BookEnabled("UN") && !_strName.StartsWith("Suite:"))
				{
					XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");

					if (_objCharacter.Options.AutomaticCopyProtection)
					{
						Gear objPlugin1 = new Gear(_objCharacter);
						TreeNode objPlugin1Node = new TreeNode();
						objPlugin1.Create(objXmlDocument.SelectSingleNode("/chummer/gears/gear[name = \"Copy Protection\"]"), objCharacter, objPlugin1Node, _intRating, null, null);
						if (_intRating == 0)
							objPlugin1.Rating = 1;
						objPlugin1.Avail = "0";
						objPlugin1.Cost = "0";
						objPlugin1.Cost3 = "0";
						objPlugin1.Cost6 = "0";
						objPlugin1.Cost10 = "0";
						objPlugin1.Capacity = "[0]";
						objPlugin1.Parent = this;
						Gears.Add(objPlugin1);
						objNode.Nodes.Add(objPlugin1Node);
					}

					if (_objCharacter.Options.AutomaticRegistration)
					{
						Gear objPlugin2 = new Gear(_objCharacter);
						TreeNode objPlugin2Node = new TreeNode();
						objPlugin2.Create(objXmlDocument.SelectSingleNode("/chummer/gears/gear[name = \"Registration\"]"), objCharacter, objPlugin2Node, 0, null, null);
						objPlugin2.Avail = "0";
						objPlugin2.Cost = "0";
						objPlugin2.Cost3 = "0";
						objPlugin2.Cost6 = "0";
						objPlugin2.Cost10 = "0";
						objPlugin2.Capacity = "[0]";
						objPlugin2.Parent = this;
						Gears.Add(objPlugin2);
						objNode.Nodes.Add(objPlugin2Node);
						objNode.Expand();
					}

					if ((objCharacter.Metatype == "A.I." || objCharacter.MetatypeCategory == "Technocritters" || objCharacter.MetatypeCategory == "Protosapients"))
					{
						Gear objPlugin3 = new Gear(_objCharacter);
						TreeNode objPlugin3Node = new TreeNode();
						objPlugin3.Create(objXmlDocument.SelectSingleNode("/chummer/gears/gear[name = \"Ergonomic\"]"), objCharacter, objPlugin3Node, 0, null, null);
						objPlugin3.Avail = "0";
						objPlugin3.Cost = "0";
						objPlugin3.Cost3 = "0";
						objPlugin3.Cost6 = "0";
						objPlugin3.Cost10 = "0";
						objPlugin3.Capacity = "[0]";
						objPlugin3.Parent = this;
						Gears.Add(objPlugin3);
						objNode.Nodes.Add(objPlugin3Node);

						Gear objPlugin4 = new Gear(_objCharacter);
						TreeNode objPlugin4Node = new TreeNode();
						objPlugin4.Create(objXmlDocument.SelectSingleNode("/chummer/gears/gear[name = \"Optimization\" and category = \"Program Options\"]"), objCharacter, objPlugin4Node, _intRating, null, null);
						if (_intRating == 0)
							objPlugin4.Rating = 1;
						objPlugin4.Avail = "0";
						objPlugin4.Cost = "0";
						objPlugin4.Cost3 = "0";
						objPlugin4.Cost6 = "0";
						objPlugin4.Cost10 = "0";
						objPlugin4.Capacity = "[0]";
						objPlugin4.Parent = this;
						Gears.Add(objPlugin4);
						objNode.Nodes.Add(objPlugin4Node);
						objNode.Expand();
					}
				}
			}
		}

		/// <summary>
		/// Copy a piece of Gear.
		/// </summary>
		/// <param name="objGear">Gear object to copy.</param>
		/// <param name="objNode">TreeNode created by copying the item.</param>
		/// <param name="objWeapons">List of Weapons created by copying the item.</param>
		/// <param name="objWeaponNodes">List of Weapon TreeNodes created by copying the item.</param>
		public void Copy(Commlink objGear, TreeNode objNode, List<Weapon> objWeapons, List<TreeNode> objWeaponNodes)
		{
			_strName = objGear.Name;
			_strCategory = objGear.Category;
			_intMaxRating = objGear.MaxRating;
			_intMinRating = objGear.MinRating;
			_intRating = objGear.Rating;
			_intQty = objGear.Quantity;
			_strCapacity = objGear.Capacity;
			_strArmorCapacity = objGear.ArmorCapacity;
			_strAvail = objGear.Avail;
			_strAvail3 = objGear.Avail3;
			_strAvail6 = objGear.Avail6;
			_strAvail10 = objGear.Avail10;
			_intCostFor = objGear.CostFor;
			_intSignal = objGear.Signal;
			_strCost = objGear.Cost;
			_strCost3 = objGear.Cost3;
			_strCost6 = objGear.Cost6;
			_strCost10 = objGear.Cost10;
			_strSource = objGear.Source;
			_strPage = objGear.Page;
			_strExtra = objGear.Extra;
			_blnBonded = objGear.Bonded;
			_blnEquipped = objGear.Equipped;
			_blnHomeNode = objGear.HomeNode;
			_nodBonus = objGear.Bonus;
            _nodConditional = objGear.Conditional;
			_nodWeaponBonus = objGear.WeaponBonus;
			_guiWeaponID = Guid.Parse(objGear.WeaponID);
			_strNotes = objGear.Notes;
			_strLocation = objGear.Location;
			_intResponse = objGear.Response;
			_intSignal = objGear.Signal;

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();

			foreach (Gear objGearChild in objGear.Gears)
			{
				TreeNode objChildNode = new TreeNode();
				Gear objChild = new Gear(_objCharacter);
				if (objGearChild.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink.Copy(objGearChild, objChildNode, objWeapons, objWeaponNodes);
					objChild = objCommlink;
				}
				else if (objGearChild.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOS = new OperatingSystem(_objCharacter);
					objOS.Copy(objGearChild, objChildNode, objWeapons, objWeaponNodes);
					objChild = objOS;
				}
				else
					objChild.Copy(objGearChild, objChildNode, objWeapons, objWeaponNodes);
				Gears.Add(objChild);

				objNode.Nodes.Add(objChildNode);
				objNode.Expand();
			}
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public new void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("gear");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("category", _strCategory);
			objWriter.WriteElementString("armorcapacity", _strArmorCapacity);
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("qty", _intQty.ToString());
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("extra", _strExtra);
			objWriter.WriteElementString("bonded", _blnBonded.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("homenode", _blnHomeNode.ToString());
			if (_nodBonus != null)
				objWriter.WriteRaw("<bonus>" + _nodBonus.InnerXml + "</bonus>");
			else
				objWriter.WriteElementString("bonus", "");
            if (_nodConditional != null)
                objWriter.WriteRaw("<conditional>" + _nodConditional.InnerText + "</conditional>");
            else
                objWriter.WriteElementString("conditional", "");
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("response", _intResponse.ToString());
			objWriter.WriteElementString("signal", _intSignal.ToString());
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteStartElement("children");
			foreach (Gear objGear in Gears)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink = (Commlink)objGear;
					objCommlink.Save(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Save(objWriter);
				}
				else
				{
					objGear.Save(objWriter);
				}
			}
			objWriter.WriteEndElement();
			objWriter.WriteElementString("location", _strLocation);
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteElementString("active", _blnActiveCommlink.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Gear from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public new void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_strCategory = objNode["category"].InnerText;
			_strArmorCapacity = objNode["armorcapacity"].InnerText;
			_intMaxRating = Convert.ToInt32(objNode["maxrating"].InnerText);
			_intRating = Convert.ToInt32(objNode["rating"].InnerText);
			_intQty = Convert.ToInt32(objNode["qty"].InnerText);
			_strAvail = objNode["avail"].InnerText;
			_strCost = objNode["cost"].InnerText;
			_strExtra = objNode["extra"].InnerText;
			_blnBonded = Convert.ToBoolean(objNode["bonded"].InnerText);
			_blnEquipped = Convert.ToBoolean(objNode["equipped"].InnerText);
			_blnHomeNode = Convert.ToBoolean(objNode["homenode"].InnerText);
			_nodBonus = objNode["bonus"];
            _nodConditional = objNode["conditional"];
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;
			_intResponse = Convert.ToInt32(objNode["response"].InnerText);
			_intSignal = Convert.ToInt32(objNode["signal"].InnerText);

			if (objNode["givenname"] != null)
				_strGivenName = objNode["givenname"].InnerText;

			if (objNode["gear"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("children/gear");
				foreach (XmlNode nodChild in nodChildren)
				{
					switch (nodChild["category"].InnerText)
					{
						case "Commlink":
						case "Commlink Upgrade":
							Commlink objCommlink = new Commlink(_objCharacter);
							objCommlink.Load(nodChild, blnCopy);
							objCommlink.Parent = this;
							Gears.Add(objCommlink);
							break;
						case "Commlink Operating System":
						case "Commlink Operating System Upgrade":
							OperatingSystem objOperatingSystem = new OperatingSystem(_objCharacter);
							objOperatingSystem.Load(nodChild, blnCopy);
							objOperatingSystem.Parent = this;
							Gears.Add(objOperatingSystem);
							break;
						default:
							Gear objGear = new Gear(_objCharacter);
							objGear.Load(nodChild, blnCopy);
							objGear.Parent = this;
							Gears.Add(objGear);
							break;
					}
				}
			}

			_strLocation = objNode["location"].InnerText;
			_strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);
			_blnActiveCommlink = Convert.ToBoolean(objNode["active"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");
				XmlNode objGearNode = objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + ExternalId + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode["translate"] != null)
						_strAltName = objGearNode["translate"].InnerText;
					if (objGearNode["altpage"] != null)
						_strAltPage = objGearNode["altpage"].InnerText;
				}

				if (_strAltName.StartsWith("Stacked Focus"))
					_strAltName = _strAltName.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));

				objGearNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode.Attributes["translate"] != null)
						_strAltCategory = objGearNode.Attributes["translate"].InnerText;
				}

				if (_strAltCategory.StartsWith("Stacked Focus"))
					_strAltCategory = _strAltCategory.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));
			}

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
				_strLocation = string.Empty;
				_blnHomeNode = false;
			}
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public new void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("gear");
			objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("name_english", _strName);
			objWriter.WriteElementString("category", DisplayCategory);
			objWriter.WriteElementString("category_english", _strCategory);
			objWriter.WriteElementString("iscommlink", true.ToString());
			objWriter.WriteElementString("ispersona", IsLivingPersona.ToString());
			objWriter.WriteElementString("isnexus", (_strCategory == "Nexus").ToString());
			objWriter.WriteElementString("isammo", (_strCategory == "Ammunition").ToString());
			objWriter.WriteElementString("isprogram", IsProgram.ToString());
			objWriter.WriteElementString("isos", false.ToString());
			objWriter.WriteElementString("issin", false.ToString());
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("qty", _intQty.ToString());
			objWriter.WriteElementString("avail", TotalAvail(true));
			objWriter.WriteElementString("avail_english", TotalAvail(true, true));
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			objWriter.WriteElementString("extra", LanguageManager.Instance.TranslateExtra(_strExtra));
			objWriter.WriteElementString("bonded", _blnBonded.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("homenode", _blnHomeNode.ToString());
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			if (_strName == "Living Persona")
				objWriter.WriteElementString("response", TotalResponse.ToString() + " (" + (TotalResponse + 1).ToString() + ")");
			else
				objWriter.WriteElementString("response", TotalResponse.ToString());
			objWriter.WriteElementString("signal", TotalSignal.ToString());
			objWriter.WriteElementString("firewall", TotalFirewall.ToString());
			objWriter.WriteElementString("system", TotalSystem.ToString());
			objWriter.WriteElementString("processorlimit", ProcessorLimit.ToString());
			objWriter.WriteElementString("conditionmonitor", ConditionMonitor.ToString());
			objWriter.WriteElementString("active", _blnActiveCommlink.ToString());
			objWriter.WriteStartElement("children");
			foreach (Gear objGear in Gears)
			{
				if (objGear.Category != "Commlink Upgrade" && objGear.Category != "Commlink Operating System Upgrade")
				{
					// Use the Gear's SubClass if applicable.
					if (objGear.GetType() == typeof(Commlink))
					{
						Commlink objCommlink = new Commlink(_objCharacter);
						objCommlink = (Commlink)objGear;
						objCommlink.Print(objWriter);
					}
					else if (objGear.GetType() == typeof(OperatingSystem))
					{
						OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
						objOperatinSystem = (OperatingSystem)objGear;
						objOperatinSystem.Print(objWriter);
					}
					else
					{
						objGear.Print(objWriter);
					}
				}
			}
			objWriter.WriteEndElement();
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Response.
		/// </summary>
		public new int Response
		{
			get
			{
				return _intResponse;
			}
			set
			{
				_intResponse = value;
			}
		}

		/// <summary>
		/// Signal.
		/// </summary>
		public new int Signal
		{
			get
			{
				return _intSignal;
			}
			set
			{
				_intSignal = value;
			}
		}

		/// <summary>
		/// Whether or not this Commlink is a Living Persona. This should only be set by the character when printing.
		/// </summary>
		public bool IsLivingPersona
		{
			get
			{
				return _blnIsLivingPersona;
			}
			set
			{
				_blnIsLivingPersona = value;
			}
		}

		/// <summary>
		/// Whether or not this Commlink is active and counting towards the character's Matrix Initiative.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return _blnActiveCommlink;
			}
			set
			{
				_blnActiveCommlink = value;
			}
		}
		#endregion

		#region Complex Properties
		/// <summary>
		/// Total Response including Commlink Upgrades.
		/// </summary>
		public int TotalResponse
		{
			get
			{
				int intResponse = _intResponse;
				foreach (Commlink objBonus in Gears.OfType<Commlink>())
				{
					if (objBonus.Category == "Commlink Upgrade" && objBonus.Response != 0)
					{
						if (objBonus.Response > intResponse)
							intResponse = objBonus.Response;
					}
				}

				// Adjust the stat to include the A.I.'s Home Node bonus.
				if (_blnHomeNode)
				{
					decimal decBonus = Math.Ceiling(_objCharacter.INT.TotalValue / 2m);
					int intBonus = Convert.ToInt32(decBonus, GlobalOptions.Instance.CultureInfo);
					intResponse += intBonus;
				}

				// If the option to adjust a Commlink's Response based on the number of programs currently running on it is enabled, determine how many are currently
				// running and adjust its value. Don't attempt to do this if the Commlink's TotalSystem is 0 since this number is needed. Items with the
				// Ergonomic Program Option also do not count towards this limit.
				if (_objCharacter.Options.CalculateCommlinkResponse && TotalSystem != 0)
				{
					double dblProcessorLimit = TotalSystem;
					double dblRunningPrograms = 0;
					foreach (Gear objProgram in Gears)
					{
						if (objProgram.IsProgram && objProgram.Equipped && (!objProgram.IsErgonomic || (objProgram.IsErgonomic && !_objCharacter.Options.ErgonomicProgramLimit)))
							dblRunningPrograms++;
					}

					int intPenalty = Convert.ToInt32(Math.Floor(dblRunningPrograms / dblProcessorLimit));
					intResponse -= intPenalty;

					// Response cannot go below 0.
					if (intResponse < 0)
						intResponse = 0;
				}

				return intResponse;
			}
		}

		/// <summary>
		/// Total Signal including Commlink Upgrades.
		/// </summary>
		public int TotalSignal
		{
			get
			{
				int intSignal = _intSignal;
				foreach (Commlink objBonus in Gears.OfType<Commlink>())
				{
					if (objBonus.Category == "Commlink Upgrade" && objBonus.Signal != 0)
					{
						if (objBonus.Signal > intSignal)
							intSignal = objBonus.Signal;
					}
				}

				// Adjust the stat to include the A.I.'s Home Node bonus.
				if (_blnHomeNode)
				{
					decimal decBonus = Math.Ceiling(_objCharacter.CHA.TotalValue / 2m);
					int intBonus = Convert.ToInt32(decBonus, GlobalOptions.Instance.CultureInfo);
					intSignal += intBonus;
				}

				return intSignal;
			}
		}

		/// <summary>
		/// Firewall.
		/// </summary>
		public new int Firewall
		{
			get
			{
				int intFirewall = 0;
				foreach (OperatingSystem objOS in Gears.OfType<OperatingSystem>())
				{
					if (objOS.Category != "Commlink Operating System Upgrade")
					{
						if (objOS.Firewall > intFirewall)
							intFirewall = objOS.Firewall;
					}
				}

				return intFirewall;
			}
		}

		/// <summary>
		/// System.
		/// </summary>
		public new int System
		{
			get
			{
				int intSystem = 0;
				foreach (OperatingSystem objOS in Gears.OfType<OperatingSystem>())
				{
					if (objOS.Category != "Commlink Operating System Upgrade")
					{
						if (objOS.System > intSystem)
							intSystem = objOS.System;
					}
				}

				return intSystem;
			}
		}

		/// <summary>
		/// Total Firewall including Commlink Operating System Upgrades.
		/// </summary>
		public int TotalFirewall
		{
			get
			{
				int intFirewall = Firewall;
				foreach (OperatingSystem objBonus in Gears.OfType<OperatingSystem>())
				{
					if (objBonus.Category == "Commlink Operating System Upgrade" && objBonus.Firewall != 0)
					{
						if (objBonus.Firewall > intFirewall)
							intFirewall = objBonus.Firewall;
					}
					foreach (OperatingSystem objPlugin in objBonus.Gears.OfType<OperatingSystem>())
					{
						if (objPlugin.Category == "Commlink Operating System Upgrade")
						{
							if (objPlugin.Firewall > intFirewall)
								intFirewall = objPlugin.Firewall;
						}
					}
				}

				// Adjust the stat to include the A.I.'s Home Node bonus.
				if (_blnHomeNode)
				{
					decimal decBonus = Math.Ceiling(_objCharacter.WIL.TotalValue / 2m);
					int intBonus = Convert.ToInt32(decBonus, GlobalOptions.Instance.CultureInfo);
					intFirewall += intBonus;
				}

				return intFirewall;
			}
		}

		/// <summary>
		/// Total System including Commlink Operating System Upgrades.
		/// </summary>
		public int TotalSystem
		{
			get
			{
				int intSystem = System;
				foreach (OperatingSystem objBonus in Gears.OfType<OperatingSystem>())
				{
					if (objBonus.Category == "Commlink Operating System Upgrade" && objBonus.System != 0)
					{
						if (objBonus.System > intSystem)
							intSystem = objBonus.System;
					}
					foreach (OperatingSystem objPlugin in objBonus.Gears.OfType<OperatingSystem>())
					{
						if (objPlugin.Category == "Commlink Operating System Upgrade")
						{
							if (objPlugin.System > intSystem)
								intSystem = objPlugin.System;
						}
					}
				}

				// Adjust the stat to include the A.I.'s Home Node bonus.
				if (_blnHomeNode)
				{
					decimal decBonus = Math.Ceiling(_objCharacter.LOG.TotalValue / 2m);
					int intBonus = Convert.ToInt32(decBonus, GlobalOptions.Instance.CultureInfo);
					intSystem += intBonus;
				}

				return intSystem;
			}
		}

		/// <summary>
		/// Commlink's Processor Limit.
		/// </summary>
		public int ProcessorLimit
		{
			get
			{
				return TotalSystem;
			}
		}

		/// <summary>
		/// Matrix Condition Monitor for the Commlink.
		/// </summary>
		public int ConditionMonitor
		{
			get
			{
				double dblSystem = Math.Ceiling(Convert.ToDouble(TotalSystem, GlobalOptions.Instance.CultureInfo) / 2);
				int intSystem = Convert.ToInt32(dblSystem, GlobalOptions.Instance.CultureInfo);
				return 8 + intSystem;
			}
		}
		#endregion
	}

	/// <summary>
	/// Commlink Operating System.
	/// </summary>
	public class OperatingSystem : Gear
	{
		private int _intFirewall = 1;
		private int _intSystem = 1;

		#region Constructor, Create, Save, Load, and Print Methods
		public OperatingSystem(Character objCharacter) : base(objCharacter)
		{
		}

		/// Create an Operating System from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlGear">XmlNode to create the object from.</param>
		/// <param name="objCharacter">Character the Gear is being added to.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		/// <param name="intRating">Selected Rating for the Gear.</param>
		/// <param name="blnAddImprovements">Whether or not Improvements should be added to the character.</param>
		/// <param name="blnCreateChildren">Whether or not child Gear should be created.</param>
		public void Create(XmlNode objXmlGear, Character objCharacter, TreeNode objNode, int intRating, bool blnAddImprovements = true, bool blnCreateChildren = true)
		{
            _guidExternalID = Guid.Parse(objXmlGear["id"].InnerText);
			_strName = objXmlGear["name"].InnerText;
			_strCategory = objXmlGear["category"].InnerText;
			_strAvail = objXmlGear["avail"].InnerText;
            _strAccuracy = objXmlGear["accuracy"].InnerText;
			if (objXmlGear["cost"] != null)
				_strCost = objXmlGear["cost"].InnerText;
            if (objXmlGear["cost3"] != null)
				_strCost3 = objXmlGear["cost3"].InnerText;
			if (objXmlGear["cost6"] != null)
				_strCost6 = objXmlGear["cost6"].InnerText;
			if (objXmlGear["cost10"] != null)
				_strCost10 = objXmlGear["cost10"].InnerText;

            _nodBonus = objXmlGear["bonus"];
            _nodConditional = objXmlGear["conditional"];
			_intMaxRating = Convert.ToInt32(objXmlGear["rating"].InnerText);
			_intRating = intRating;
			_strSource = objXmlGear["source"].InnerText;
			_strPage = objXmlGear["page"].InnerText;
			_intFirewall = Convert.ToInt32(objXmlGear["firewall"].InnerText);
			_intSystem = Convert.ToInt32(objXmlGear["system"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");
				XmlNode objGearNode = objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + ExternalId + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode["translate"] != null)
						_strAltName = objGearNode["translate"].InnerText;
					if (objGearNode["altpage"] != null)
						_strAltPage = objGearNode["altpage"].InnerText;
				}

				if (_strAltName.StartsWith("Stacked Focus"))
					_strAltName = _strAltName.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));

				objGearNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode.Attributes["translate"] != null)
						_strAltCategory = objGearNode.Attributes["translate"].InnerText;
				}

				if (_strAltCategory.StartsWith("Stacked Focus"))
					_strAltCategory = _strAltCategory.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));
			}

			string strSource = _guiID.ToString();

			objNode.Text = DisplayNameShort;
			objNode.Tag = _guiID.ToString();

			// If the item grants a bonus, pass the information to the Improvement Manager.
			if (objXmlGear["bonus"] != null)
			{
				ImprovementManager objImprovementManager;
				if (blnAddImprovements)
					objImprovementManager = new ImprovementManager(objCharacter);
				else
					objImprovementManager = new ImprovementManager(null);

				if (!objImprovementManager.CreateImprovements(Improvement.ImprovementSource.Gear, strSource, objXmlGear["bonus"], false, 1, DisplayNameShort))
				{
					_guiID = Guid.Empty;
					return;
				}
				if (objImprovementManager.SelectedValue != "")
				{
					_strExtra = objImprovementManager.SelectedValue;
					objNode.Text += " (" + objImprovementManager.SelectedValue + ")";
				}
			}
		}

		/// <summary>
		/// Copy a piece of Gear.
		/// </summary>
		/// <param name="objGear">Gear object to copy.</param>
		/// <param name="objNode">TreeNode created by copying the item.</param>
		/// <param name="objWeapons">List of Weapons created by copying the item.</param>
		/// <param name="objWeaponNodes">List of Weapon TreeNodes created by copying the item.</param>
		public void Copy(OperatingSystem objGear, TreeNode objNode, List<Weapon> objWeapons, List<TreeNode> objWeaponNodes)
		{
			_strName = objGear.Name;
			_strCategory = objGear.Category;
			_intMaxRating = objGear.MaxRating;
			_intMinRating = objGear.MinRating;
			_intRating = objGear.Rating;
			_intQty = objGear.Quantity;
			_strCapacity = objGear.Capacity;
			_strArmorCapacity = objGear.ArmorCapacity;
			_strAvail = objGear.Avail;
			_strAvail3 = objGear.Avail3;
			_strAvail6 = objGear.Avail6;
			_strAvail10 = objGear.Avail10;
			_intCostFor = objGear.CostFor;
			_intSignal = objGear.Signal;
			_strCost = objGear.Cost;
			_strCost3 = objGear.Cost3;
			_strCost6 = objGear.Cost6;
			_strCost10 = objGear.Cost10;
			_strSource = objGear.Source;
			_strPage = objGear.Page;
			_strExtra = objGear.Extra;
			_blnBonded = objGear.Bonded;
			_blnEquipped = objGear.Equipped;
			_blnHomeNode = objGear.HomeNode;
			_nodBonus = objGear.Bonus;
            _nodConditional = objGear.Conditional;
			_nodWeaponBonus = objGear.WeaponBonus;
			_guiWeaponID = Guid.Parse(objGear.WeaponID);
			_strNotes = objGear.Notes;
			_strLocation = objGear.Location;
			_intSystem = objGear.System;
			_intFirewall = objGear.Firewall;

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();

			foreach (Gear objGearChild in objGear.Gears)
			{
				TreeNode objChildNode = new TreeNode();
				Gear objChild = new Gear(_objCharacter);
				if (objGearChild.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink.Copy(objGearChild, objChildNode, objWeapons, objWeaponNodes);
					objChild = objCommlink;
				}
				else if (objGearChild.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOS = new OperatingSystem(_objCharacter);
					objOS.Copy(objGearChild, objChildNode, objWeapons, objWeaponNodes);
					objChild = objOS;
				}
				else
					objChild.Copy(objGearChild, objChildNode, objWeapons, objWeaponNodes);
				Gears.Add(objChild);

				objNode.Nodes.Add(objChildNode);
				objNode.Expand();
			}
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public new void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("gear");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("category", _strCategory);
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("qty", _intQty.ToString());
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("extra", _strExtra);
			objWriter.WriteElementString("bonded", _blnBonded.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("homenode", _blnHomeNode.ToString());
			if (_nodBonus != null)
				objWriter.WriteRaw("<bonus>" + _nodBonus.InnerXml + "</bonus>");
			else
				objWriter.WriteElementString("bonus", "");
            if (_nodConditional != null)
                objWriter.WriteRaw("<conditional>" + _nodConditional.InnerText + "</bonus>");
            else
                objWriter.WriteElementString("conditional", "");
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("firewall", _intFirewall.ToString());
			objWriter.WriteElementString("system", _intSystem.ToString());
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteStartElement("children");
			foreach (Gear objGear in Gears)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink = (Commlink)objGear;
					objCommlink.Save(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Save(objWriter);
				}
				else
				{
					objGear.Save(objWriter);
				}
			}
			objWriter.WriteEndElement();
			objWriter.WriteElementString("location", _strLocation);
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Gear from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public new void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_strCategory = objNode["category"].InnerText;
			_intMaxRating = Convert.ToInt32(objNode["maxrating"].InnerText);
			_intRating = Convert.ToInt32(objNode["rating"].InnerText);
			_intQty = Convert.ToInt32(objNode["qty"].InnerText);
			_strAvail = objNode["avail"].InnerText;
			_strCost = objNode["cost"].InnerText;
			_strExtra = objNode["extra"].InnerText;
			_blnBonded = Convert.ToBoolean(objNode["bonded"].InnerText);
			_blnEquipped = Convert.ToBoolean(objNode["equipped"].InnerText);
			_blnHomeNode = Convert.ToBoolean(objNode["homenode"].InnerText);
			_nodBonus = objNode["bonus"];
            _nodConditional = objNode["conditional"];
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;
			_intFirewall = Convert.ToInt32(objNode["firewall"].InnerText);
			_intSystem = Convert.ToInt32(objNode["system"].InnerText);

			if (objNode["gear"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("children/gear");
				foreach (XmlNode nodChild in nodChildren)
				{
					switch (nodChild["category"].InnerText)
					{
						case "Commlink":
						case "Commlink Upgrade":
							Commlink objCommlink = new Commlink(_objCharacter);
							objCommlink.Load(nodChild, blnCopy);
							objCommlink.Parent = this;
							Gears.Add(objCommlink);
							break;
						case "Commlink Operating System":
						case "Commlink Operating System Upgrade":
							OperatingSystem objOperatingSystem = new OperatingSystem(_objCharacter);
							objOperatingSystem.Load(nodChild, blnCopy);
							objOperatingSystem.Parent = this;
							Gears.Add(objOperatingSystem);
							break;
						default:
							Gear objGear = new Gear(_objCharacter);
							objGear.Load(nodChild, blnCopy);
							objGear.Parent = this;
							Gears.Add(objGear);
							break;
					}
				}
			}

			_strLocation = objNode["location"].InnerText;
			_strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);
			_strGivenName = objNode["givenname"].InnerText;

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");
				XmlNode objGearNode = objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + ExternalId + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode["translate"] != null)
						_strAltName = objGearNode["translate"].InnerText;
					if (objGearNode["altpage"] != null)
						_strAltPage = objGearNode["altpage"].InnerText;
				}

				if (_strAltName.StartsWith("Stacked Focus"))
					_strAltName = _strAltName.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));

				objGearNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objGearNode != null)
				{
					if (objGearNode.Attributes["translate"] != null)
						_strAltCategory = objGearNode.Attributes["translate"].InnerText;
				}

				if (_strAltCategory.StartsWith("Stacked Focus"))
					_strAltCategory = _strAltCategory.Replace("Stacked Focus", LanguageManager.Instance.GetString("String_StackedFocus"));
			}

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
				_strLocation = string.Empty;
				_blnHomeNode = false;
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public new void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("gear");
			objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("name_english", _strName);
			objWriter.WriteElementString("category", DisplayCategory);
			objWriter.WriteElementString("category_english", _strCategory);
			objWriter.WriteElementString("iscommlink", false.ToString());
			objWriter.WriteElementString("ispersona", false.ToString());
			objWriter.WriteElementString("isnexus", (_strCategory == "Nexus").ToString());
			objWriter.WriteElementString("isammo", (_strCategory == "Ammunition").ToString());
			objWriter.WriteElementString("isprogram", IsProgram.ToString());
			if (_strCategory.EndsWith("Upgrade"))
				objWriter.WriteElementString("isos", false.ToString());
			else
				objWriter.WriteElementString("isos", true.ToString());
			objWriter.WriteElementString("issin", false.ToString());
			objWriter.WriteElementString("maxrating", _intMaxRating.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("qty", _intQty.ToString());
			objWriter.WriteElementString("avail", TotalAvail(true));
			objWriter.WriteElementString("avail_english", TotalAvail(true, true));
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			objWriter.WriteElementString("extra", LanguageManager.Instance.TranslateExtra(_strExtra));
			objWriter.WriteElementString("bonded", _blnBonded.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
			objWriter.WriteElementString("homenode", _blnHomeNode.ToString());
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			objWriter.WriteElementString("firewall", _intFirewall.ToString());
			objWriter.WriteElementString("system", _intSystem.ToString());
			objWriter.WriteStartElement("children");
			foreach (Gear objGear in Gears)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink = (Commlink)objGear;
					objCommlink.Print(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Print(objWriter);
				}
				else
				{
					objGear.Print(objWriter);
				}
			}
			objWriter.WriteEndElement();
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Firewall.
		/// </summary>
		public new int Firewall
		{
			get
			{
				return _intFirewall;
			}
			set
			{
				_intFirewall = value;
			}
		}

		/// <summary>
		/// System.
		/// </summary>
		public new int System
		{
			get
			{
				return _intSystem;
			}
			set
			{
				_intSystem = value;
			}
		}
		#endregion
	}

	/// <summary>
	/// Vehicle Modification.
	/// </summary>
	public class VehicleMod : Equipment
	{
		private string _strLimit = "";
		private string _strSlots = "0";
		private string _strMaxRating = "0";
		private bool _blnIncludeInVehicle = false;
		private int _intResponse = 0;
		private int _intSystem = 0;
		private int _intFirewall = 0;
		private int _intSignal = 0;
		private int _intPilot = 0;
		private string _strSubsystems = "";
        private bool _blnWeaponMount = false;
        private bool _blnModularElectronics = false;

		// Variables used to calculate the Mod's cost from the Vehicle.
		private int _intVehicleCost = 0;
		private int _intBody = 0;
		private int _intSpeed = 0;
		private int _intAccelRunning = 0;
		private int _intAccelWalking = 0;

		#region Constructor, Create, Save, Load, and Print Methods
		public VehicleMod(Character objCharacter) : base(objCharacter)
		{
			// Create the GUID for the new VehicleMod.
			_guiID = Guid.NewGuid();
		}

		/// Create a Vehicle Modification from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlMod">XmlNode to create the object from.</param>
		/// <param name="objNode">TreeNode to populate a TreeView.</param>
		/// <param name="intRating">Selected Rating for the Gear.</param>
		public void Create(XmlNode objXmlMod, TreeNode objNode, int intRating)
		{
            _guidExternalID = Guid.Parse(objXmlMod["id"].InnerText);
			_strName = objXmlMod["name"].InnerText;
			_strCategory = objXmlMod["category"].InnerText;
			if (objXmlMod["limit"] != null)
				_strLimit = objXmlMod["limit"].InnerText;

			_strSlots = objXmlMod["slots"].InnerText;
			if (intRating != 0)
				_intRating = Convert.ToInt32(intRating);

            if (objXmlMod["rating"] != null)
				_strMaxRating = objXmlMod["rating"].InnerText;
			else
				_strMaxRating = "0";
			
            if (objXmlMod["response"] != null)
				_intResponse = Convert.ToInt32(objXmlMod["response"].InnerText);
			if (objXmlMod["system"] != null)
				_intSystem = Convert.ToInt32(objXmlMod["system"].InnerText);
			if (objXmlMod["firewall"] != null)
				_intFirewall = Convert.ToInt32(objXmlMod["firewall"].InnerText);
			if (objXmlMod["signal"] != null)
				_intSignal = Convert.ToInt32(objXmlMod["signal"].InnerText);
			if (objXmlMod["pilot"] != null)
				_intPilot = Convert.ToInt32(objXmlMod["pilot"].InnerText);
            if (objXmlMod["modularelectronics"] != null)
                _blnModularElectronics = Convert.ToBoolean(objXmlMod["modularelectronics"].InnerText);

            // Add Subsytem information if applicable.
			if (objXmlMod["subsystems"] != null)
			{
				string strSubsystem = "";
				foreach (XmlNode objXmlSubsystem in objXmlMod.SelectNodes("subsystems/subsystem"))
				{
					strSubsystem += objXmlSubsystem.InnerText + ",";
				}
				_strSubsystems = strSubsystem;
			}
			_strAvail = objXmlMod["avail"].InnerText;
			
			// Check for a Variable Cost.
			if (objXmlMod["cost"] != null)
			{
                if (objXmlMod["cost"].InnerText.StartsWith("Variable"))
                    _strCost = VariableCost(objXmlMod["cost"].InnerText, DisplayNameShort).ToString();
                else
                    _strCost = objXmlMod["cost"].InnerText;
			}

            if (objXmlMod["weaponmount"] != null)
                _blnWeaponMount = Convert.ToBoolean(objXmlMod["weaponmount"].InnerText);

			_strSource = objXmlMod["source"].InnerText;
			_strPage = objXmlMod["page"].InnerText;
			if (objXmlMod["bonus"] != null)
				_nodBonus = objXmlMod["bonus"];
            if (objXmlMod["conditional"] != null)
                _nodConditional = objXmlMod["conditional"];

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("vehicles.xml");
				XmlNode objModNode = objXmlDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + ExternalId + "\"]");
				if (objModNode != null)
				{
					if (objModNode["translate"] != null)
						_strAltName = objModNode["translate"].InnerText;
					if (objModNode["altpage"] != null)
						_strAltPage = objModNode["altpage"].InnerText;
				}

				objModNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objModNode != null)
				{
					if (objModNode.Attributes["translate"] != null)
						_strAltCategory = objModNode.Attributes["translate"].InnerText;
				}
			}

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("mod");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("category", _strCategory);
			objWriter.WriteElementString("limit", _strLimit);
			objWriter.WriteElementString("slots", _strSlots);
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("maxrating", _strMaxRating);
			objWriter.WriteElementString("response", _intResponse.ToString());
			objWriter.WriteElementString("system", _intSystem.ToString());
			objWriter.WriteElementString("firewall", _intFirewall.ToString());
			objWriter.WriteElementString("signal", _intSignal.ToString());
			objWriter.WriteElementString("pilot", _intPilot.ToString());
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("extra", _strExtra);
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("included", _blnIncludeInVehicle.ToString());
			objWriter.WriteElementString("equipped", _blnEquipped.ToString());
            objWriter.WriteElementString("weaponmount", _blnWeaponMount.ToString());
			objWriter.WriteElementString("subsystems", _strSubsystems);
            if (_blnModularElectronics)
                objWriter.WriteElementString("modularelectronics", _blnModularElectronics.ToString());
			objWriter.WriteStartElement("weapons");
			foreach (Weapon objWeapon in Weapons)
				objWeapon.Save(objWriter);
			objWriter.WriteEndElement();
			if (Cyberwares.Count > 0)
			{
				objWriter.WriteStartElement("cyberwares");
				foreach (Cyberware objCyberware in Cyberwares)
					objCyberware.Save(objWriter);
				objWriter.WriteEndElement();
			}
            if (_nodBonus != null)
                objWriter.WriteRaw(_nodBonus.OuterXml);
            if (_nodConditional != null)
                objWriter.WriteRaw(_nodConditional.OuterXml);
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the VehicleMod from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public override void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_strCategory = objNode["category"].InnerText;
			_strLimit = objNode["limit"].InnerText;
			_strSlots = objNode["slots"].InnerText;
			_intRating = Convert.ToInt32(objNode["rating"].InnerText);
			_strMaxRating = objNode["maxrating"].InnerText;
			_intResponse = Convert.ToInt32(objNode["response"].InnerText);
			_intSystem = Convert.ToInt32(objNode["system"].InnerText);
			_intFirewall = Convert.ToInt32(objNode["firewall"].InnerText);
			_intSignal = Convert.ToInt32(objNode["signal"].InnerText);
			_intPilot = Convert.ToInt32(objNode["pilot"].InnerText);
			_strAvail = objNode["avail"].InnerText;
			_strCost = objNode["cost"].InnerText;
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;
			_blnIncludeInVehicle = Convert.ToBoolean(objNode["included"].InnerText);
			_blnEquipped = Convert.ToBoolean(objNode["equipped"].InnerText);
            _blnWeaponMount = Convert.ToBoolean(objNode["weaponmount"].InnerText);
			_strSubsystems = objNode["subsystems"].InnerText;

            if (objNode["modularelectronics"] != null)
                _blnModularElectronics = Convert.ToBoolean(objNode["modularelectronics"].InnerText);

			if (objNode["weapons"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("weapons/weapon");
				foreach (XmlNode nodChild in nodChildren)
				{
					Weapon objWeapon = new Weapon(_objCharacter);
					objWeapon.Load(nodChild, blnCopy);
					objWeapon.Parent = this;
					Weapons.Add(objWeapon);
				}
			}
			if (objNode["cyberwares"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("cyberwares/cyberware");
				foreach (XmlNode nodChild in nodChildren)
				{
					Cyberware objCyberware = new Cyberware(_objCharacter);
					objCyberware.Load(nodChild, blnCopy);
					objCyberware.Parent = this;
					Cyberwares.Add(objCyberware);
				}
			}
			try
			{
				_nodBonus = objNode["bonus"];
			}
			catch
			{
			}
            try
            {
                _nodConditional = objNode["conditional"];
            }
            catch
            {
            }

			_strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);
			_strExtra = objNode["extra"].InnerText;

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("vehicles.xml");
				XmlNode objModNode = objXmlDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + ExternalId + "\"]");
				if (objModNode != null)
				{
					if (objModNode["translate"] != null)
						_strAltName = objModNode["translate"].InnerText;
					if (objModNode["altpage"] != null)
						_strAltPage = objModNode["altpage"].InnerText;
				}

				objModNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objModNode != null)
				{
					if (objModNode.Attributes["translate"] != null)
						_strAltCategory = objModNode.Attributes["translate"].InnerText;
				}
			}

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("mod");
			objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("category", DisplayCategory);
			objWriter.WriteElementString("limit", _strLimit);
			objWriter.WriteElementString("slots", _strSlots);
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("avail", TotalAvail);
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			objWriter.WriteElementString("included", _blnIncludeInVehicle.ToString());
			objWriter.WriteStartElement("weapons");
			foreach (Weapon objWeapon in Weapons)
				objWeapon.Print(objWriter);
			objWriter.WriteEndElement();
			objWriter.WriteStartElement("cyberwares");
			foreach (Cyberware objCyberware in Cyberwares)
				objCyberware.Print(objWriter);
			objWriter.WriteEndElement();
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Which Vehicle types the Mod is limited to.
		/// </summary>
		public string Limit
		{
			get
			{
				return _strLimit;
			}
			set
			{
				_strLimit = value;
			}
		}

		/// <summary>
		/// Number of Slots the Mod uses.
		/// </summary>
		public string Slots
		{
			get
			{
				return _strSlots;
			}
			set
			{
				_strSlots = value;
			}
		}

		/// <summary>
		/// Response.
		/// </summary>
		public int Response
		{
			get
			{
				return _intResponse;
			}
			set
			{
				_intResponse = value;
			}
		}

		/// <summary>
		/// System.
		/// </summary>
		public int System
		{
			get
			{
				return _intSystem;
			}
			set
			{
				_intSystem = value;
			}
		}

		/// <summary>
		/// Firewall.
		/// </summary>
		public int Firewall
		{
			get
			{
				return _intFirewall;
			}
			set
			{
				_intFirewall = value;
			}
		}

		/// <summary>
		/// Signal.
		/// </summary>
		public int Signal
		{
			get
			{
				return _intSignal;
			}
			set
			{
				_intSignal = value;
			}
		}

		/// <summary>
		/// Pilot.
		/// </summary>
		public int Pilot
		{
			get
			{
				return _intPilot;
			}
			set
			{
				_intPilot = value;
			}
		}

		/// <summary>
		/// Cost.
		/// </summary>
		public string Cost
		{
			get
			{
				return _strCost;
			}
			set
			{
				_strCost = value;
			}
		}

		// Properties used to calculate the Mod's cost from the Vehicle.
		public int VehicleCost
		{
			get
			{
				return _intVehicleCost;
			}
			set
			{
				_intVehicleCost = value;
			}
		}

        /// <summary>
        /// Amount that the Vehicle Mod modifies the Vehicle's Body value.
        /// </summary>
		public int Body
		{
			get
			{
				return _intBody;
			}
			set
			{
				_intBody = value;
			}
		}

        /// <summary>
        /// Amount that the Vehicle Mod modifies the Vehicle's Speed value.
        /// </summary>
		public int Speed
		{
			get
			{
				return _intSpeed;
			}
			set
			{
				_intSpeed = value;
			}
		}

        /// <summary>
        /// Amount that the Vehicle Mod modifies the Vehicle's Running Acceleration value.
        /// </summary>
		public int AccelRunning
		{
			get
			{
				return _intAccelRunning;
			}
			set
			{
				_intAccelRunning = value;
			}
		}

        /// <summary>
        /// Amount that the Vehicle Mod modifies the Vehicle's Walking Acceleration value.
        /// </summary>
		public int AccelWalking
		{
			get
			{
				return _intAccelWalking;
			}
			set
			{
				_intAccelWalking = value;
			}
		}

		/// <summary>
		/// Whether or not the Vehicle Mod allows Cyberware Plugins.
		/// </summary>
		public bool AllowCyberware
		{
			get
			{
				if (_strSubsystems == "")
					return false;
				else
					return true;
			}
		}

		/// <summary>
		/// Allowed Cyberwarre Subsystems.
		/// </summary>
		public string Subsystems
		{
			get
			{
				return _strSubsystems;
			}
			set
			{
				_strSubsystems = value;
			}
		}

        /// <summary>
        /// Whether or not the Vehicle Mod can be used to mount Weapons.
        /// </summary>
        public bool WeaponMount
        {
            get
            {
                return _blnWeaponMount;
            }
            set
            {
                _blnWeaponMount = value;
            }
        }

        /// <summary>
        /// Whether or not the Vehicle Mod grants access to Modular Electronics.
        /// </summary>
        public bool ModularElectronics
        {
            get
            {
                return _blnModularElectronics;
            }
        }
		#endregion

		#region Complex Properties
		/// <summary>
		/// Total cost of the VehicleMod.
		/// </summary>
		public int TotalCost
		{
			get
			{
				int intReturn = 0;

				// If the cost is determined by the Rating, evaluate the expression.
				XmlDocument objXmlDocument = new XmlDocument();
				XPathNavigator nav = objXmlDocument.CreateNavigator();

				string strCost = "";
				string strCostExpression = "";
				strCostExpression = _strCost;

				strCost = strCostExpression.Replace("Rating", _intRating.ToString());
				strCost = strCost.Replace("Vehicle Cost", _intVehicleCost.ToString());
				// If the Body is 0 (Microdrone), treat it as 2 for the purposes of determine Modification cost.
				if (_intBody > 0)
					strCost = strCost.Replace("Body", _intBody.ToString());
				else
					strCost = strCost.Replace("Body", "2");
				strCost = strCost.Replace("Speed", _intSpeed.ToString());
				strCost = strCost.Replace("Accel(Running)", _intAccelRunning.ToString());
				strCost = strCost.Replace("Accel", _intAccelWalking.ToString());
				XPathExpression xprCost = nav.Compile(strCost);
				intReturn = Convert.ToInt32(nav.Evaluate(xprCost), GlobalOptions.Instance.CultureInfo);

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				// Retrieve the price of VehicleWeapons.
				foreach (Weapon objWeapon in Weapons)
				{
					intReturn += objWeapon.TotalCost;
				}

				// Retrieve the price of Cyberware.
				foreach (Cyberware objCyberware in Cyberwares)
					intReturn += objCyberware.TotalCost;

				return intReturn;
			}
		}

		/// <summary>
		/// The cost of just the Vehicle Mod itself.
		/// </summary>
		public int OwnCost
		{
			get
			{
				int intReturn = 0;

				// If the cost is determined by the Rating, evaluate the expression.
				XmlDocument objXmlDocument = new XmlDocument();
				XPathNavigator nav = objXmlDocument.CreateNavigator();

				string strCost = "";
				string strCostExpression = "";
				strCostExpression = _strCost;

				strCost = strCostExpression.Replace("Rating", _intRating.ToString());
				strCost = strCost.Replace("Vehicle Cost", _intVehicleCost.ToString());
				// If the Body is 0 (Microdrone), treat it as 2 for the purposes of determine Modification cost.
				if (_intBody > 0)
					strCost = strCost.Replace("Body", _intBody.ToString());
				else
					strCost = strCost.Replace("Body", "2");
				strCost = strCost.Replace("Speed", _intSpeed.ToString());
				strCost = strCost.Replace("Accel(Running)", _intAccelRunning.ToString());
				strCost = strCost.Replace("Accel", _intAccelWalking.ToString());
				XPathExpression xprCost = nav.Compile(strCost);
				intReturn = Convert.ToInt32(nav.Evaluate(xprCost).ToString());

				if (DiscountCost)
					intReturn = Convert.ToInt32(Convert.ToDouble(intReturn, GlobalOptions.Instance.CultureInfo) * 0.9);

				return intReturn;
			}
		}

		/// <summary>
		/// The number of Slots the Mod consumes.
		/// </summary>
		public int CalculatedSlots
		{
			get
			{
				return Convert.ToInt32(_strSlots.Replace("Rating", _intRating.ToString()));
			}
		}

		/// <summary>
		/// Whether or not the Mod is allowed to accept Cyberware Modular Plugins.
		/// </summary>
		public bool AllowModularPlugins
		{
			get
			{
				bool blnReturn = false;
				foreach (Cyberware objChild in Cyberwares)
				{
					if (objChild.Subsytems.Contains("Modular Plug-In"))
					{
						blnReturn = true;
						break;
					}
				}

				return blnReturn;
			}
		}
		#endregion
	}

	/// <summary>
	/// Vehicle.
	/// </summary>
	public class Vehicle : Equipment
	{
		private int _intHandling = 0;
		private string _strAccel = "";
		private int _intSpeed = 0;
		private int _intPilot = 0;
		private int _intBody = 0;
		private int _intArmor = 0;
		private int _intSensor = 0;
		private int _intAddSlots = 0;
		private int _intDeviceRating = 3;
		private bool _blnHomeNode = false;
		private List<string> _lstLocations = new List<string>();

		// Condition Monitor Progress.
		private int _intPhysicalCMFilled = 0;

		#region Constructor, Create, Save, Load, and Print Methods
		public Vehicle(Character objCharacter) : base(objCharacter)
		{
			// Create the GUID for the new Vehicle.
			_guiID = Guid.NewGuid();
		}

		/// Create a Vehicle from an XmlNode and return the TreeNodes for it.
		/// <param name="objXmlVehicle">XmlNode of the Vehicle to create.</param>
		/// <param name="objNode">TreeNode to add to a TreeView.</param>
		/// <param name="cmsVehicle">ContextMenuStrip to attach to Weapon Mounts.</param>
		/// <param name="cmsVehicleGear">ContextMenuStrip to attach to Gear.</param>
		/// <param name="cmsVehicleWeapon">ContextMenuStrip to attach to Vehicle Weapons.</param>
		/// <param name="cmsVehicleWeaponAccessory">ContextMenuStrip to attach to Weapon Accessories.</param>
		/// <param name="cmsVehicleWeaponMod">ContextMenuStrip to attachk to Weapon Mods.</param>
		/// <param name="blnCreateChildren">Whether or not child items should be created.</param>
		public void Create(XmlNode objXmlVehicle, TreeNode objNode, ContextMenuStrip cmsVehicle, ContextMenuStrip cmsVehicleGear, ContextMenuStrip cmsVehicleWeapon, ContextMenuStrip cmsVehicleWeaponAccessory, ContextMenuStrip cmsVehicleWeaponMod, bool blnCreateChildren = true)
		{
            _guidExternalID = Guid.Parse(objXmlVehicle["id"].InnerText);
			_strName = objXmlVehicle["name"].InnerText;
			_strCategory = objXmlVehicle["category"].InnerText;
			_intHandling = Convert.ToInt32(objXmlVehicle["handling"].InnerText);
			_strAccel = objXmlVehicle["accel"].InnerText;
			_intSpeed = Convert.ToInt32(objXmlVehicle["speed"].InnerText);
			_intPilot = Convert.ToInt32(objXmlVehicle["pilot"].InnerText);
			_intBody = Convert.ToInt32(objXmlVehicle["body"].InnerText);
			_intArmor = Convert.ToInt32(objXmlVehicle["armor"].InnerText);
			_intSensor = Convert.ToInt32(objXmlVehicle["sensor"].InnerText);
			_intDeviceRating = Convert.ToInt32(objXmlVehicle["devicerating"].InnerText);
			_strAvail = objXmlVehicle["avail"].InnerText;
			_strSource = objXmlVehicle["source"].InnerText;
			_strPage = objXmlVehicle["page"].InnerText;

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("vehicles.xml");
				XmlNode objVehicleNode = objXmlDocument.SelectSingleNode("/chummer/vehicles/vehicle[id = \"" + ExternalId + "\"]");
				if (objVehicleNode != null)
				{
					if (objVehicleNode["translate"] != null)
						_strAltName = objVehicleNode["translate"].InnerText;
					if (objVehicleNode["altpage"] != null)
						_strAltPage = objVehicleNode["altpage"].InnerText;
				}

				objVehicleNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objVehicleNode != null)
				{
					if (objVehicleNode.Attributes["translate"] != null)
						_strAltCategory = objVehicleNode.Attributes["translate"].InnerText;
				}
			}

			objNode.Text = DisplayName;
			objNode.Tag = _guiID.ToString();

            // Check for Variable Cost.
            if (objXmlVehicle["cost"].InnerText.StartsWith("Variable"))
                _strCost = VariableCost(objXmlVehicle["cost"].InnerText, DisplayNameShort).ToString();
            else
                _strCost = objXmlVehicle["cost"].InnerText;

			// If there are any VehicleMods that come with the Vehicle, add them.
			if (objXmlVehicle["mods"] != null && blnCreateChildren)
			{
				XmlDocument objXmlDocument = new XmlDocument();
				objXmlDocument = XmlManager.Instance.Load("vehicles.xml");

				XmlNodeList objXmlModList = objXmlVehicle.SelectNodes("mods/id");
				foreach (XmlNode objXmlVehicleMod in objXmlModList)
				{
					XmlNode objXmlMod = objXmlDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + objXmlVehicleMod.InnerText + "\"]");
					TreeNode objModNode = new TreeNode();
					VehicleMod objMod = new VehicleMod(_objCharacter);
					int intRating = 0;

					if (objXmlVehicleMod.Attributes["rating"] != null)
						intRating = Convert.ToInt32(objXmlVehicleMod.Attributes["rating"].InnerText);

					if (objXmlVehicleMod.Attributes["select"] != null)
						objMod.Extra = objXmlVehicleMod.Attributes["select"].InnerText;

					objMod.Create(objXmlMod, objModNode, intRating);
					objMod.IncludedInParent = true;

					VehicleMods.Add(objMod);
					objModNode.ForeColor = SystemColors.GrayText;
					objModNode.ContextMenuStrip = cmsVehicle;

					objNode.Nodes.Add(objModNode);
					objNode.Expand();
				}
				if (objXmlVehicle.SelectSingleNode("mods/addslots") != null)
					_intAddSlots = Convert.ToInt32(objXmlVehicle.SelectSingleNode("mods/addslots").InnerText);
			}

			// If there is any Gear that comes with the Vehicle, add them.
			if (objXmlVehicle["gears"] != null && blnCreateChildren)
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");

				XmlNodeList objXmlGearList = objXmlVehicle.SelectNodes("gears/id");
				foreach (XmlNode objXmlVehicleGear in objXmlGearList)
				{
					XmlNode objXmlGaer = objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + objXmlVehicleGear.InnerText + "\"]");
					TreeNode objGearNode = new TreeNode();
					Gear objGear = new Gear(_objCharacter);
					int intRating = 0;
					int intQty = 1;
					string strForceValue = "";

					if (objXmlVehicleGear.Attributes["rating"] != null)
						intRating = Convert.ToInt32(objXmlVehicleGear.Attributes["rating"].InnerText);

					if (objXmlVehicleGear.Attributes["qty"] != null)
						intQty = Convert.ToInt32(objXmlVehicleGear.Attributes["qty"].InnerText);

					if (objXmlVehicleGear.Attributes["select"] != null)
						strForceValue = objXmlVehicleGear.Attributes["select"].InnerText;
					else
						strForceValue = "";

					List<Weapon> objWeapons = new List<Weapon>();
					List<TreeNode> objWeaponNodes = new List<TreeNode>();
					objGear.Create(objXmlGaer, _objCharacter, objGearNode, intRating, objWeapons, objWeaponNodes, strForceValue);
					objGear.Cost = "0";
					objGear.Quantity = intQty;
					objGear.MaxRating = objGear.Rating;
					objGearNode.Text = objGear.DisplayName;
					objGearNode.ContextMenuStrip = cmsVehicleGear;

					foreach (Weapon objWeapon in objWeapons)
						objWeapon.VehicleMounted = true;

					Gears.Add(objGear);

					objNode.Nodes.Add(objGearNode);
					objNode.Expand();
				}
			}

			// If there are any Weapons that come with the Vehicle, add them.
			if (objXmlVehicle["weapons"] != null && blnCreateChildren)
			{
				XmlDocument objXmlWeaponDocument = XmlManager.Instance.Load("weapons.xml");

				foreach (XmlNode objXmlWeapon in objXmlVehicle.SelectNodes("weapons/id"))
				{
					bool blnAttached = false;
					TreeNode objWeaponNode = new TreeNode();
					Weapon objWeapon = new Weapon(_objCharacter);

					XmlNode objXmlWeaponNode = objXmlWeaponDocument.SelectSingleNode("/chummer/weapons/weapon[id = \"" + objXmlWeapon["id"].InnerText + "\"]");
					objWeapon.Create(objXmlWeaponNode, _objCharacter, objWeaponNode, cmsVehicleWeapon, cmsVehicleWeaponAccessory, cmsVehicleWeaponMod);
					objWeapon.Cost = 0;
					objWeapon.VehicleMounted = true;

					// Find the first free Weapon Mount in the Vehicle.
					foreach (VehicleMod objMod in VehicleMods)
					{
						if (objMod.WeaponMount && objMod.Weapons.Count == 0)
						{
							objMod.Weapons.Add(objWeapon);
							foreach (TreeNode objModNode in objNode.Nodes)
							{
								if (objModNode.Tag.ToString() == objMod.InternalId)
								{
									objWeaponNode.ContextMenuStrip = cmsVehicleWeapon;
									objModNode.Nodes.Add(objWeaponNode);
									objModNode.Expand();
									blnAttached = true;
									break;
								}
							}
							break;
						}
					}

					// If a free Weapon Mount could not be found, just attach it to the first one found and let the player deal with it.
					if (!blnAttached)
					{
						foreach (VehicleMod objMod in VehicleMods)
						{
							if (objMod.WeaponMount)
							{
								objMod.Weapons.Add(objWeapon);
								foreach (TreeNode objModNode in objNode.Nodes)
								{
									if (objModNode.Tag.ToString() == objMod.InternalId)
									{
										objWeaponNode.ContextMenuStrip = cmsVehicleWeapon;
										objModNode.Nodes.Add(objWeaponNode);
										objModNode.Expand();
										blnAttached = true;
										break;
									}
								}
								break;
							}
						}
					}

					// Look for Weapon Accessories.
					if (objXmlWeapon["accessories"] != null)
					{
						foreach (XmlNode objXmlAccessory in objXmlWeapon.SelectNodes("accessories/id"))
						{
							XmlNode objXmlAccessoryNode = objXmlWeaponDocument.SelectSingleNode("/chummer/accessories/accessory[name = \"" + objXmlAccessory["name"].InnerText + "\"]");
							WeaponAccessory objMod = new WeaponAccessory(_objCharacter);
							TreeNode objModNode = new TreeNode();
							string strMount = "";
							if (objXmlAccessory["mount"] != null)
								strMount = objXmlAccessory["mount"].InnerText;
							objMod.Create(objXmlAccessoryNode, objModNode, strMount);
							objMod.Cost = "0";
							objModNode.ContextMenuStrip = cmsVehicleWeaponAccessory;

							objWeapon.WeaponAccessories.Add(objMod);

							objWeaponNode.Nodes.Add(objModNode);
							objWeaponNode.Expand();
						}
					}

					// Look for Weapon Mods.
					if (objXmlWeapon["mods"] != null)
					{
						foreach (XmlNode objXmlMod in objXmlWeapon.SelectNodes("mods/id"))
						{
							XmlNode objXmlModNode = objXmlWeaponDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + objXmlMod["id"].InnerText + "\"]");
							WeaponMod objMod = new WeaponMod(_objCharacter);
							TreeNode objModNode = new TreeNode();
							objMod.Create(objXmlModNode, objModNode);
							objMod.Cost = "0";
							objModNode.ContextMenuStrip = cmsVehicleWeaponMod;

							objWeapon.WeaponMods.Add(objMod);

							objWeaponNode.Nodes.Add(objModNode);
							objWeaponNode.Expand();
						}
					}
				}
			}
		}

		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("vehicle");
            objWriter.WriteElementString("eguid", _guidExternalID.ToString());
			objWriter.WriteElementString("guid", _guiID.ToString());
			objWriter.WriteElementString("name", _strName);
			objWriter.WriteElementString("category", _strCategory);
			objWriter.WriteElementString("handling", _intHandling.ToString());
			objWriter.WriteElementString("accel", _strAccel);
			objWriter.WriteElementString("speed", _intSpeed.ToString());
			objWriter.WriteElementString("pilot", _intPilot.ToString());
			objWriter.WriteElementString("body", _intBody.ToString());
			objWriter.WriteElementString("armor", _intArmor.ToString());
			objWriter.WriteElementString("sensor", _intSensor.ToString());
			objWriter.WriteElementString("devicerating", _intDeviceRating.ToString());
			objWriter.WriteElementString("avail", _strAvail);
			objWriter.WriteElementString("cost", _strCost);
			objWriter.WriteElementString("addslots", _intAddSlots.ToString());
			objWriter.WriteElementString("source", _strSource);
			objWriter.WriteElementString("page", _strPage);
			objWriter.WriteElementString("physicalcmfilled", _intPhysicalCMFilled.ToString());
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("homenode", _blnHomeNode.ToString());
			objWriter.WriteStartElement("mods");
			foreach (VehicleMod objMod in VehicleMods)
				objMod.Save(objWriter);
			objWriter.WriteEndElement();
			objWriter.WriteStartElement("gears");
			foreach (Gear objGear in Gears)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink = (Commlink)objGear;
					objCommlink.Save(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Save(objWriter);
				}
				else
				{
					objGear.Save(objWriter);
				}
			}
			objWriter.WriteEndElement();
			objWriter.WriteStartElement("weapons");
			foreach (Weapon objWeapon in Weapons)
				objWeapon.Save(objWriter);
			objWriter.WriteEndElement();
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteElementString("discountedcost", DiscountCost.ToString());
			if (_lstLocations.Count > 0)
			{
				// <locations>
				objWriter.WriteStartElement("locations");
				foreach (string strLocation in _lstLocations)
				{
					objWriter.WriteElementString("location", strLocation);
				}
				// </locations>
				objWriter.WriteEndElement();
			}
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Vehicle from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public override void Load(XmlNode objNode, bool blnCopy = false)
		{
            _guidExternalID = Guid.Parse(objNode["eguid"].InnerText);
			_guiID = Guid.Parse(objNode["guid"].InnerText);
			_strName = objNode["name"].InnerText;
			_strCategory = objNode["category"].InnerText;
			_intHandling = Convert.ToInt32(objNode["handling"].InnerText);
			_strAccel = objNode["accel"].InnerText;
			_intSpeed = Convert.ToInt32(objNode["speed"].InnerText);
			_intPilot = Convert.ToInt32(objNode["pilot"].InnerText);
			_intBody = Convert.ToInt32(objNode["body"].InnerText);
			_intArmor = Convert.ToInt32(objNode["armor"].InnerText);
			_intSensor = Convert.ToInt32(objNode["sensor"].InnerText);
			_intDeviceRating = Convert.ToInt32(objNode["devicerating"].InnerText);
			_strAvail = objNode["avail"].InnerText;
			_strCost = objNode["cost"].InnerText;
			_intAddSlots = Convert.ToInt32(objNode["addslots"].InnerText);
			_strSource = objNode["source"].InnerText;
			_strPage = objNode["page"].InnerText;
			_intPhysicalCMFilled = Convert.ToInt32(objNode["physicalcmfilled"].InnerText);
			_strGivenName = objNode["givenname"].InnerText;
			_blnHomeNode = Convert.ToBoolean(objNode["homenode"].InnerText);

			if (GlobalOptions.Instance.Language != "en-us")
			{
				XmlDocument objXmlDocument = XmlManager.Instance.Load("vehicles.xml");
				XmlNode objVehicleNode = objXmlDocument.SelectSingleNode("/chummer/vehicles/vehicle[name = \"" + _strName + "\"]");
				if (objVehicleNode != null)
				{
					if (objVehicleNode["translate"] != null)
						_strAltName = objVehicleNode["translate"].InnerText;
					if (objVehicleNode["altpage"] != null)
						_strAltPage = objVehicleNode["altpage"].InnerText;
				}

				objVehicleNode = objXmlDocument.SelectSingleNode("/chummer/categories/category[. = \"" + _strCategory + "\"]");
				if (objVehicleNode != null)
				{
					if (objVehicleNode.Attributes["translate"] != null)
						_strAltCategory = objVehicleNode.Attributes["translate"].InnerText;
				}
			}

			if (objNode["mods"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("mods/mod");
				foreach (XmlNode nodChild in nodChildren)
				{
					VehicleMod objMod = new VehicleMod(_objCharacter);
					objMod.Load(nodChild, blnCopy);
					objMod.Parent = this;
					VehicleMods.Add(objMod);
				}
			}

			if (objNode["gears"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("gears/gear");
				foreach (XmlNode nodChild in nodChildren)
				{
					switch (nodChild["category"].InnerText)
					{
						case "Commlink":
						case "Commlink Upgrade":
							Commlink objCommlink = new Commlink(_objCharacter);
							objCommlink.Load(nodChild, blnCopy);
							objCommlink.Parent = this;
							Gears.Add(objCommlink);
							break;
						case "Commlink Operating System":
						case "Commlink Operating System Upgrade":
							OperatingSystem objOperatingSystem = new OperatingSystem(_objCharacter);
							objOperatingSystem.Load(nodChild, blnCopy);
							objOperatingSystem.Parent = this;
							Gears.Add(objOperatingSystem);
							break;
						default:
							Gear objGear = new Gear(_objCharacter);
							objGear.Load(nodChild, blnCopy);
							objGear.Parent = this;
							Gears.Add(objGear);
							break;
					}
				}
			}

			if (objNode["weapons"] != null)
			{
				XmlNodeList nodChildren = objNode.SelectNodes("weapons/weapon");
				foreach (XmlNode nodChild in nodChildren)
				{
					Weapon objWeapon = new Weapon(_objCharacter);
					objWeapon.Load(nodChild, blnCopy);
					objWeapon.VehicleMounted = true;
					objWeapon.Parent = this;
					if (objWeapon.Weapons.Count > 0)
					{
						foreach (Weapon objUnderbarrel in objWeapon.Weapons)
							objUnderbarrel.VehicleMounted = true;
					}
					Weapons.Add(objWeapon);
				}
			}

			_strNotes = objNode["notes"].InnerText;
			_blnDiscountCost = Convert.ToBoolean(objNode["discountedcost"].InnerText);

			if (objNode["locations"] != null)
			{
				// Locations.
				foreach (XmlNode objXmlLocation in objNode.SelectNodes("locations/location"))
				{
					_lstLocations.Add(objXmlLocation.InnerText);
				}
			}

			if (blnCopy)
			{
				_guiID = Guid.NewGuid();
				_blnHomeNode = false;
			}
		}

		/// <summary>
		/// Print the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public override void Print(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("vehicle");
			objWriter.WriteElementString("name", DisplayNameShort);
			objWriter.WriteElementString("category", DisplayCategory);
			objWriter.WriteElementString("handling", TotalHandling.ToString());
			objWriter.WriteElementString("accel", TotalAccel);
			objWriter.WriteElementString("speed", TotalSpeed.ToString());
			objWriter.WriteElementString("pilot", Pilot.ToString());
			objWriter.WriteElementString("body", TotalBody.ToString());
			objWriter.WriteElementString("armor", TotalArmor.ToString());
			if (_objCharacter.Options.UseCalculatedVehicleSensorRatings)
				objWriter.WriteElementString("sensor", CalculatedSensor.ToString());
			else
				objWriter.WriteElementString("sensor", _intSensor.ToString());
			objWriter.WriteElementString("sensorsignal", SensorSignal.ToString());
			objWriter.WriteElementString("avail", TotalAvail);
			objWriter.WriteElementString("cost", TotalCost.ToString());
			objWriter.WriteElementString("owncost", OwnCost.ToString());
			objWriter.WriteElementString("source", _objCharacter.Options.LanguageBookShort(_strSource));
			objWriter.WriteElementString("page", Page);
			objWriter.WriteElementString("physicalcm", PhysicalCM.ToString());
			objWriter.WriteElementString("physicalcmfilled", _intPhysicalCMFilled.ToString());
			objWriter.WriteElementString("givenname", _strGivenName);
			objWriter.WriteElementString("firewall", Firewall.ToString());
			objWriter.WriteElementString("signal", Signal.ToString());
			objWriter.WriteElementString("response", Response.ToString());
			objWriter.WriteElementString("system", System.ToString());
			objWriter.WriteElementString("devicerating", DeviceRating.ToString());
			objWriter.WriteElementString("maneuver", Maneuver.ToString());
			objWriter.WriteElementString("homenode", _blnHomeNode.ToString());
			objWriter.WriteStartElement("mods");
			foreach (VehicleMod objMod in VehicleMods)
				objMod.Print(objWriter);
			objWriter.WriteEndElement();
			objWriter.WriteStartElement("gears");
			foreach (Gear objGear in Gears)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(_objCharacter);
					objCommlink = (Commlink)objGear;
					objCommlink.Print(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(_objCharacter);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Print(objWriter);
				}
				else
				{
					objGear.Print(objWriter);
				}
			}
			objWriter.WriteEndElement();
			objWriter.WriteStartElement("weapons");
			foreach (Weapon objWeapon in Weapons)
				objWeapon.Print(objWriter);
			objWriter.WriteEndElement();
			if (_objCharacter.Options.PrintNotes)
				objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Handling.
		/// </summary>
		public int Handling
		{
			get
			{
				return _intHandling;
			}
			set
			{
				_intHandling = value;
			}
		}

		/// <summary>
		/// Acceleration.
		/// </summary>
		public string Accel
		{
			get
			{
				return _strAccel;
			}
			set
			{
				_strAccel = value;
			}
		}

		/// <summary>
		/// Speed.
		/// </summary>
		public int Speed
		{
			get
			{
				return _intSpeed;
			}
			set
			{
				_intSpeed = value;
			}
		}

		/// <summary>
		/// Pilot.
		/// </summary>
		public int Pilot
		{
			get
			{
				int intReturn = _intPilot;
				foreach (VehicleMod objMod in VehicleMods)
				{
					if (objMod.Pilot > intReturn && objMod.Equipped)
						intReturn = objMod.Pilot;
				}
				return intReturn;
			}
			set
			{
				_intPilot = value;
			}
		}

		/// <summary>
		/// Body.
		/// </summary>
		public int Body
		{
			get
			{
				return _intBody;
			}
			set
			{
				_intBody = value;
			}
		}

		/// <summary>
		/// Armor.
		/// </summary>
		public int Armor
		{
			get
			{
				return _intArmor;
			}
			set
			{
				_intArmor = value;
			}
		}

		/// <summary>
		/// Sensor.
		/// </summary>
		public int Sensor
		{
			get
			{
				return _intSensor;
			}
			set
			{
				_intSensor = value;
			}
		}

		/// <summary>
		/// Device Rating.
		/// </summary>
		public int DeviceRating
		{
			get
			{
				int intDeviceRating = _intDeviceRating;

				foreach (VehicleMod objMod in VehicleMods)
				{
					if (objMod.Bonus != null)
					{
						// Add the Modification's Device Rating to the Vehicle's base Device Rating.
						if (objMod.Bonus["devicerating"] != null)
							intDeviceRating += Convert.ToInt32(objMod.Bonus["devicerating"].InnerText);
					}
				}

				// Device Rating cannot go below 1.
				if (intDeviceRating < 1)
					intDeviceRating = 1;

				return intDeviceRating;
			}
			set
			{
				_intDeviceRating = value;
			}
		}

		/// <summary>
		/// Physical Condition Monitor boxes.
		/// </summary>
		public int PhysicalCM
		{
			get
			{
				return 8 + Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_intBody, GlobalOptions.Instance.CultureInfo) / 2.0));
			}
		}

		/// <summary>
		/// Physical Condition Monitor boxes filled.
		/// </summary>
		public int PhysicalCMFilled
		{
			get
			{
				return _intPhysicalCMFilled;
			}
			set
			{
				_intPhysicalCMFilled = value;
			}
		}

		/// <summary>
		/// Cost.
		/// </summary>
		public string Cost
		{
			get
			{
				return _strCost;
			}
			set
			{
				_strCost = value;
			}
		}

		/// <summary>
		/// Acceleration "Walking" speed (first number of the set).
		/// </summary>
		public int AccelWalking
		{
			get
			{
				string[] strAccel = _strAccel.Split('/');
				return Convert.ToInt32(strAccel[0]);
			}
		}

		/// <summary>
		/// Acceleration "Running" speed (second number of the set).
		/// </summary>
		public int AccelRunning
		{
			get
			{
				string[] strAccel = _strAccel.Split('/');
				return Convert.ToInt32(strAccel[1]);
			}
		}

		/// <summary>
		/// Number of Slots the Vehicle has for Modifications.
		/// </summary>
		public int Slots
		{
			get
			{
				// A Vehicle has 4 or BODY slots, whichever is higher.
				if (TotalBody > 4)
					return TotalBody + _intAddSlots;
				else
					return 4 + _intAddSlots;
			}
		}

		/// <summary>
		/// Response.
		/// </summary>
		public int Response
		{
			get
			{
				int intReturn = _intDeviceRating;
				foreach (VehicleMod objMod in VehicleMods)
				{
					if (objMod.Response > intReturn && objMod.Equipped)
						intReturn = objMod.Response;
				}

				// Adjust the stat to include the A.I.'s Home Node bonus.
				if (_blnHomeNode)
				{
					decimal decBonus = Math.Ceiling(_objCharacter.INT.TotalValue / 2m);
					int intBonus = Convert.ToInt32(decBonus, GlobalOptions.Instance.CultureInfo);
					intReturn += intBonus;
				}

				return intReturn;
			}
		}

		/// <summary>
		/// System.
		/// </summary>
		public int System
		{
			get
			{
				int intReturn = _intDeviceRating;
				foreach (VehicleMod objMod in VehicleMods)
				{
					if (objMod.System > intReturn && objMod.Equipped)
						intReturn = objMod.System;
				}

				// Adjust the stat to include the A.I.'s Home Node bonus.
				if (_blnHomeNode)
				{
					decimal decBonus = Math.Ceiling(_objCharacter.LOG.TotalValue / 2m);
					int intBonus = Convert.ToInt32(decBonus, GlobalOptions.Instance.CultureInfo);
					intReturn += intBonus;
				}

				return intReturn;
			}
		}

		/// <summary>
		/// Firewall.
		/// </summary>
		public int Firewall
		{
			get
			{
				int intReturn = _intDeviceRating;
				foreach (VehicleMod objMod in VehicleMods)
				{
					if (objMod.Firewall > intReturn && objMod.Equipped)
						intReturn = objMod.Firewall;
				}

				// Adjust the stat to include the A.I.'s Home Node bonus.
				if (_blnHomeNode)
				{
					decimal decBonus = Math.Ceiling(_objCharacter.WIL.TotalValue / 2m);
					int intBonus = Convert.ToInt32(decBonus, GlobalOptions.Instance.CultureInfo);
					intReturn += intBonus;
				}

				return intReturn;
			}
		}

		/// <summary>
		/// Signal.
		/// </summary>
		public int Signal
		{
			get
			{
				int intReturn = _intDeviceRating;
				foreach (VehicleMod objMod in VehicleMods)
				{
					if (objMod.Signal > intReturn && objMod.Equipped)
						intReturn = objMod.Signal;
				}

				// Adjust the stat to include the A.I.'s Home Node bonus.
				if (_blnHomeNode)
				{
					decimal decBonus = Math.Ceiling(_objCharacter.CHA.TotalValue / 2m);
					int intBonus = Convert.ToInt32(decBonus, GlobalOptions.Instance.CultureInfo);
					intReturn += intBonus;
				}

				return intReturn;
			}
		}

		/// <summary>
		/// Sensor's Signal.
		/// </summary>
		public int SensorSignal
		{
			get
			{
				int intReturn = 0;
				foreach (Gear objGear in Gears)
				{
					if (objGear.Category == "Sensors" && objGear.Signal > 0 && objGear.Signal > intReturn)
						intReturn = objGear.Signal;
				}
				return intReturn;
			}
		}

		/// <summary>
		/// Calculate the Vehicle's Sensor Rating based on the items within its Sensor.
		/// </summary>
		public int CalculatedSensor
		{
			get
			{
				int intReturn = 0;
				int intCount = 0;
				foreach (Gear objGear in Gears)
				{
					if (objGear.Category == "Sensors" && objGear.Signal > 0)
					{
						foreach (Gear objChild in objGear.Gears)
						{
							if (objChild.Category == "Sensor Functions")
							{
								if (objChild.Rating > 0)
								{
									intCount++;
									intReturn += objChild.Rating;
								}
							}
						}
					}
					break;
				}

				if (intCount > 0)
				{
					decimal decReturn = Convert.ToDecimal(intReturn, GlobalOptions.Instance.CultureInfo);
					intReturn = Convert.ToInt32(Math.Ceiling(decReturn / intCount));
				}
				else
					intReturn = _intSensor;

				return intReturn;
			}
		}

		/// <summary>
		/// Whether or not the Vehicle is an A.I.'s Home Node.
		/// </summary>
		public bool HomeNode
		{
			get
			{
				return _blnHomeNode;
			}
			set
			{
				_blnHomeNode = value;
			}
		}

		/// <summary>
		/// Locations.
		/// </summary>
		public List<string> Locations
		{
			get
			{
				return _lstLocations;
			}
		}
		#endregion

		#region Complex Properties
		/// <summary>
		/// The number of Slots on the Vehicle that are used by Mods.
		/// </summary>
		public int SlotsUsed
		{
			get
			{
				int intSlotsUsed = 0;
				foreach (VehicleMod objMod in VehicleMods)
				{
					// Mods that are included with a Vehicle by default do not count toward the Slots used.
					if (!objMod.IncludedInParent && objMod.Equipped)
						intSlotsUsed += objMod.CalculatedSlots;
				}

				return intSlotsUsed;
			}
		}

		/// <summary>
		/// Total cost of the Vehicle including all after-market Modification.
		/// </summary>
		public int TotalCost
		{
			get
			{
				int intCost = Convert.ToInt32(_strCost);

				if (DiscountCost)
					intCost = Convert.ToInt32(Convert.ToDouble(intCost, GlobalOptions.Instance.CultureInfo) * 0.9);

				foreach (VehicleMod objMod in VehicleMods)
				{
					// Do not include the price of Mods that are part of the base configureation.
					if (!objMod.IncludedInParent)
					{
						objMod.VehicleCost = Convert.ToInt32(_strCost);
						objMod.Body = _intBody;
						objMod.Speed = _intSpeed;
						objMod.AccelRunning = AccelRunning;
						objMod.AccelWalking = AccelWalking;

						intCost += objMod.TotalCost;
					}
					else
					{
						// If the Mod is a part of the base config, check the items attached to it since their cost still counts.
						foreach (Weapon objWeapon in objMod.Weapons)
							intCost += objWeapon.TotalCost;
						foreach (Cyberware objCyberware in objMod.Cyberwares)
							intCost += objCyberware.TotalCost;
					}
				}

				foreach (Gear objGear in Gears)
				{
					intCost += objGear.TotalCost;
				}

				return intCost;
			}
		}

		/// <summary>
		/// The cost of just the Vehicle itself.
		/// </summary>
		public int OwnCost
		{
			get
			{
				int intCost = Convert.ToInt32(_strCost);

				if (DiscountCost)
					intCost = Convert.ToInt32(Convert.ToDouble(intCost, GlobalOptions.Instance.CultureInfo) * 0.9);

				return intCost;
			}
		}

		/// <summary>
		/// Total Speed of the Vehicle including Modifications.
		/// </summary>
		public int TotalSpeed
		{
			get
			{
				decimal decSpeed = Convert.ToDecimal(_intSpeed, GlobalOptions.Instance.CultureInfo);

				foreach (VehicleMod objMod in VehicleMods)
				{
					if (!objMod.IncludedInParent && objMod.Equipped && objMod.Bonus != null)
					{
						// Multiply the Vehicle's base Speed by the Modification's Speed multiplier.
						if (objMod.Bonus["speed"] != null)
							decSpeed += (Convert.ToDecimal(_intSpeed, GlobalOptions.Instance.CultureInfo) * Convert.ToDecimal(objMod.Bonus["speed"].InnerText, GlobalOptions.Instance.CultureInfo));
					}
				}

				// If the Vehicle's Total Armor exceeds its Total Body, Accel and Speed are decreased by 20%.
				// The value must also exceed the Armor Rating that the Vehicles comes equipped with by default.
				if (TotalArmor > TotalBody && TotalArmor > _intArmor)
				{
					decSpeed -= (Convert.ToDecimal(_intSpeed, GlobalOptions.Instance.CultureInfo) * 0.2m);
				}

				// Make sure Speed doesn't go below 0.
				if (decSpeed < 0.0m)
					decSpeed = 0.0m;

				return Convert.ToInt32(Math.Ceiling(decSpeed));
			}
		}

		/// <summary>
		/// Total Accel of the Vehicle including Modifications.
		/// </summary>
		public string TotalAccel
		{
			get
			{
				decimal decAccelWalking = Convert.ToDecimal(AccelWalking, GlobalOptions.Instance.CultureInfo);
				decimal decAccelRunning = Convert.ToDecimal(AccelRunning, GlobalOptions.Instance.CultureInfo);

				foreach (VehicleMod objMod in VehicleMods)
				{
					if (!objMod.IncludedInParent && objMod.Equipped && objMod.Bonus != null)
					{
						// Multiply the Vehicle's base Accel by the Modification's Accel multiplier.
						if (objMod.Bonus["accel"] != null)
						{
							if (objMod.Bonus["accel"].InnerText.Contains("+"))
							{
								string[] strAccel = objMod.Bonus["accel"].InnerText.Split('/');

								XmlDocument objXmlDocument = new XmlDocument();
								XPathNavigator nav = objXmlDocument.CreateNavigator();

								XPathExpression xprWalking = nav.Compile(strAccel[0].Replace("Rating", objMod.Rating.ToString()).Replace("+", string.Empty));
								XPathExpression xprRunning = nav.Compile(strAccel[1].Replace("Rating", objMod.Rating.ToString()).Replace("+", string.Empty));

								decAccelWalking += Convert.ToDecimal(nav.Evaluate(xprWalking), GlobalOptions.Instance.CultureInfo);
								decAccelRunning += Convert.ToDecimal(nav.Evaluate(xprRunning), GlobalOptions.Instance.CultureInfo);
							}
							else
							{
								decAccelWalking += (Convert.ToDecimal(AccelWalking, GlobalOptions.Instance.CultureInfo) * Convert.ToDecimal(objMod.Bonus["accel"].InnerText, GlobalOptions.Instance.CultureInfo));
								decAccelRunning += (Convert.ToDecimal(AccelRunning, GlobalOptions.Instance.CultureInfo) * Convert.ToDecimal(objMod.Bonus["accel"].InnerText, GlobalOptions.Instance.CultureInfo));
							}
						}
					}
				}

				// If the Vehicle's Total Armor exceeds its Total Body, Accel and Speed are decreased by 20%.
				// The value must also exceed the Armor Rating that the Vehicles comes equipped with by default.
				if (TotalArmor > TotalBody && TotalArmor > _intArmor)
				{
					decAccelWalking -= (Convert.ToDecimal(AccelWalking, GlobalOptions.Instance.CultureInfo) * 0.2m);
					decAccelRunning -= (Convert.ToDecimal(AccelRunning, GlobalOptions.Instance.CultureInfo) * 0.2m);
				}

				// Make sure Acceleration doesn't go below 0.
				if (decAccelWalking < 0.0m)
					decAccelWalking = 0.0m;
				if (decAccelRunning < 0.0m)
					decAccelRunning = 0.0m;

				return Convert.ToInt32(Math.Ceiling(decAccelWalking)).ToString() + "/" + Convert.ToInt32(Math.Ceiling(decAccelRunning)).ToString();
			}
		}

		/// <summary>
		/// Total Body of the Vehicle including Modifications.
		/// </summary>
		public int TotalBody
		{
			get
			{
				int intBody = _intBody;

				foreach (VehicleMod objMod in VehicleMods)
				{
					if (!objMod.IncludedInParent && objMod.Equipped && objMod.Bonus != null)
					{
						// Add the Modification's Body to the Vehicle's base Body.
						if (objMod.Bonus["body"] != null)
							intBody += Convert.ToInt32(objMod.Bonus["body"].InnerText);
					}
				}

				return intBody;
			}
		}

		/// <summary>
		/// Total Handling of the Vehicle including Modifications.
		/// </summary>
		public int TotalHandling
		{
			get
			{
				int intHandling = _intHandling;

				foreach (VehicleMod objMod in VehicleMods)
				{
					if (!objMod.IncludedInParent && objMod.Equipped && objMod.Bonus != null)
					{
						// Add the Modification's Handling to the Vehicle's base Handling.
						if (objMod.Bonus["handling"] != null)
							intHandling += Convert.ToInt32(objMod.Bonus["handling"].InnerText);
					}
				}

				return intHandling;
			}
		}

		/// <summary>
		/// Total Armor of the Vehicle including Modifications.
		/// </summary>
		public int TotalArmor
		{
			get
			{
				int intBaseArmor = _intArmor;
				int intModArmor = 0;

				foreach (VehicleMod objMod in VehicleMods)
				{
					if (!objMod.IncludedInParent && objMod.Equipped && objMod.Bonus != null)
					{
						// Add the Modification's Armor to the Vehicle's base Armor. Armor provided by Mods strips the Vehicle of its base Armor.
						if (objMod.Bonus["armor"] != null)
						{
							intBaseArmor = 0;
							intModArmor += Math.Min(MaxArmor, Convert.ToInt32(objMod.Bonus["armor"].InnerText.Replace("Rating", objMod.Rating.ToString())));
						}
					}
				}

				return intBaseArmor + intModArmor;
			}
		}

		/// <summary>
		/// Maximum amount of each Armor type the Vehicle can hold.
		/// </summary>
		public int MaxArmor
		{
			get
			{
				int intReturn = 0;
				int intMultiplier = 2;

				// Drones are allowed up to Body x 3 Armor.
				if (_strCategory.StartsWith("Drones:"))
					intMultiplier = 3;

				intReturn = TotalBody * intMultiplier;

				// If ignoring the rules, do not limit Armor to the Vehicle's standard rules.
				if (_objCharacter.IgnoreRules)
					intReturn = 20;

				return intReturn;
			}
		}

		/// <summary>
		/// Vehicle's Maneuver AutoSoft Rating.
		/// </summary>
		public int Maneuver
		{
			get
			{
				int intReturn = 0;
                CommonFunctions objFunctions = new CommonFunctions(_objCharacter);
                Gear objGear = (Gear)objFunctions.FindEquipmentByExternalId(Constants.ManeuverAutosoft, Gears, typeof(Gear));
				if (objGear != null)
					intReturn = objGear.Rating;

				return intReturn;
			}
		}

		/// <summary>
		/// Whether or not the Vehicle has the Modular Electronics Vehicle Modification installed.
		/// </summary>
		public bool HasModularElectronics()
		{
			bool blnReturn = false;
			foreach (VehicleMod objMod in VehicleMods)
			{
				if (objMod.ModularElectronics)
					blnReturn = true;
			}
			return blnReturn;
		}
		#endregion
	}
}