using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FSPlay.KiemVu.Entities.Config
{
	/// <summary>
	/// Định nghĩa danh hiệu của người chơi
	/// </summary>
	public class KTitleXML
	{
		/// <summary>
		/// ID danh hiệu
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Tên danh hiệu
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Mô tả danh hiệu
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Thời gian tồn tại (giờ)
		/// </summary>
		public int Duration { get; set; }
		
		/// <summary>
		/// Có hiệu ứng
		/// </summary>
		public string IsAnimation { get; set; }
		
		/// <summary>
		/// Thời gian thực thi hiệu ứng danh hiệu
		/// </summary>
		public float AnimationSpeed { get; set; }
		
		/// <summary>
		/// Độ co dãn danh hiệu
		/// </summary>
		public float Scale { get; set; }
		
		/// <summary>
		/// Đường dẫn file Bundle chứa ảnh
		/// </summary>
		public string BundleDir { get; set; }
		
		/// <summary>
        /// Đường dẫn file Atlas chứa ảnh
        /// </summary>
        public string AtlasName { get; set; }

        /// <summary>
        /// Danh sách Sprite tạo nên hiệu ứng
        /// </summary>
        public List<string> SpriteNames { get; set; }

		/// <summary>
		/// Chuyển đối tượng từ XMLNode
		/// </summary>
		/// <param name="xmlNode"></param>
		/// <returns></returns>
		public static KTitleXML Parse(XElement xmlNode)
		{
			KTitleXML titleInfo = new KTitleXML()
			{
				ID = int.Parse(xmlNode.Attribute("ID").Value),
				Text = xmlNode.Attribute("Text").Value,
				Description = xmlNode.Attribute("Description").Value,
				Duration = int.Parse(xmlNode.Attribute("Duration").Value),
				SpriteNames = new List<string>(),
			};
			
			// Kiểm tra và parse các thuộc tính tùy chọn
    		XAttribute isAnimationAttr = xmlNode.Attribute("IsAnimation");
    		if (isAnimationAttr != null)
    		{
    		    titleInfo.IsAnimation = isAnimationAttr.Value;
    		}

    		// Nếu IsAnimation là true, lấy các thuộc tính khác
    		if (titleInfo.IsAnimation != null)
    		{
        		// Lấy và parse AnimationSpeed nếu có
        		XAttribute animationSpeedAttr = xmlNode.Attribute("AnimationSpeed");
        		if (animationSpeedAttr != null)
        		{
        		    titleInfo.AnimationSpeed = float.Parse(animationSpeedAttr.Value);
        		}

        		// Lấy và parse Scale nếu có
        		XAttribute scaleAttr = xmlNode.Attribute("Scale");
        		if (scaleAttr != null)
        		{
        		    titleInfo.Scale = float.Parse(scaleAttr.Value);
        		}

        		// Lấy BundleDir nếu có
        		XAttribute bundleDirAttr = xmlNode.Attribute("BundleDir");
        		if (bundleDirAttr != null)
        		{
        		    titleInfo.BundleDir = bundleDirAttr.Value;
        		}

        		// Lấy AtlasName nếu có
        		XAttribute atlasNameAttr = xmlNode.Attribute("AtlasName");
        		if (atlasNameAttr != null)
        		{
        		    titleInfo.AtlasName = atlasNameAttr.Value;
        		}

        		// Parse các Sprite names
        		foreach (XElement node in xmlNode.Elements("Sprite"))
        		{
        		    string spriteName = node.Attribute("Name").Value;
        		    titleInfo.SpriteNames.Add(spriteName);
        		}
    		}

    		return titleInfo;

		}
	}
}

