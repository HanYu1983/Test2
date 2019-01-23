using UnityEngine;
namespace Remix{
	public class ItemNoteEng{
		public const int ID_COUNT = 58;

		public string ID {get; set;}
public string Name {get; set;}
public string Desc {get; set;}

		public static ItemNoteEng Get(string key){
			switch (key) {
			case "I10010": return new ItemNoteEng{ID="I10010",Name="Can",Desc="Use in Interactive Mode or Gameplay recover HP50"};
case "I10020": return new ItemNoteEng{ID="I10020",Name="Milk",Desc="Use in Interactive Mode or Gameplay recover HP150"};
case "I10030": return new ItemNoteEng{ID="I10030",Name="Dry",Desc="Use in Interactive Mode or Gameplay recover HP300"};
case "I10040": return new ItemNoteEng{ID="I10040",Name="Cat Grass",Desc="Use in Interactive Mode or Gameplay recover HP500"};
case "I10050": return new ItemNoteEng{ID="I10050",Name="Dried Sardine",Desc="Use in Interactive Mode or Gameplay recover HP250"};
case "I10060": return new ItemNoteEng{ID="I10060",Name="Bonito",Desc="Use in Interactive Mode or Gameplay recover HP500"};
case "I10070": return new ItemNoteEng{ID="I10070",Name="Fish",Desc="Use in Interactive Mode or Gameplay recover HP500 and add EXP500"};
case "I10080": return new ItemNoteEng{ID="I10080",Name="Food Pouches",Desc="Use in Interactive Mode or Gameplay recover HP1000 and add EXP1000"};
case "I20010": return new ItemNoteEng{ID="I20010",Name="Digital camera",Desc="Use in Explore Mode receive a photo"};
case "I20020": return new ItemNoteEng{ID="I20020",Name="Instant camera",Desc="Use in Explore Mode receive a photo in half time"};
case "I20030": return new ItemNoteEng{ID="I20030",Name="Action camera",Desc="Use in Explore Mode receive a photo immediately"};
case "I20040": return new ItemNoteEng{ID="I20040",Name="EVIL camera",Desc="Use in Explore Mode receive a photo and 20% receive a Panorama"};
case "I20050": return new ItemNoteEng{ID="I20050",Name="DSLR camera",Desc="Use in Explore Mode receive a photo and 40% receive a Panorama"};
case "I20060": return new ItemNoteEng{ID="I20060",Name="Folding camera",Desc="Use in Explore Mode receive a photo and 50% receive second photo"};
case "I20070": return new ItemNoteEng{ID="I20070",Name="TLR Camera ",Desc="Use in Explore Mode receive two photos"};
case "I20080": return new ItemNoteEng{ID="I20080",Name="ETRS camera",Desc="Use in Explore Mode receive two photos and 40% receive a Panorama"};
case "I21010": return new ItemNoteEng{ID="I21010",Name="Selfie stick",Desc="Use in Explore Mode reduce 50% waiting time"};
case "I21020": return new ItemNoteEng{ID="I21020",Name="Tripod",Desc="Use in Explore Mode reduce 100% waiting time"};
case "I30010": return new ItemNoteEng{ID="I30010",Name="Laser pointer",Desc="Use in Interactive Mode with adult cat can add EXP50"};
case "I30020": return new ItemNoteEng{ID="I30020",Name="Mouse cat toy",Desc="Use in Interactive Mode with adult cat can add EXP200"};
case "I30030": return new ItemNoteEng{ID="I30030",Name="取消中"};
case "I30040": return new ItemNoteEng{ID="I30040",Name="Teaser wand",Desc="Use in Interactive Mode with kitten can add EXP50"};
case "I30050": return new ItemNoteEng{ID="I30050",Name="Wool ball",Desc="Use in Interactive Mode with kitten can add EXP200"};
case "I30060": return new ItemNoteEng{ID="I30060",Name="取消中"};
case "I30070": return new ItemNoteEng{ID="I30070",Name="Scratcher",Desc="Use in Interactive Mode with fat cat can add EXP50"};
case "I30080": return new ItemNoteEng{ID="I30080",Name="heater",Desc="Use in Interactive Mode with fat cat can add EXP200"};
case "I30090": return new ItemNoteEng{ID="I30090",Name="取消中"};
case "I30100": return new ItemNoteEng{ID="I30100",Name="取消中"};
case "I30110": return new ItemNoteEng{ID="I30110",Name="取消中"};
case "I31010": return new ItemNoteEng{ID="I31010",Name="Catnip Spray",Desc="Use in Explore Mode or Interactive Mode reduce 50% waiting time"};
case "I31020": return new ItemNoteEng{ID="I31020",Name="Dried Catnip",Desc="Use in Explore Mode or Interactive Mode reduce 100% waiting time"};
case "I32010": return new ItemNoteEng{ID="I32010",Name="Cardboard Car",Desc="Use in Explore Mode capture an adult cat"};
case "I32020": return new ItemNoteEng{ID="I32020",Name="Robotic vacuum",Desc="Use in Explore Mode capture a kitten"};
case "I32030": return new ItemNoteEng{ID="I32030",Name="Sofa bed",Desc="Use in Explore Mode capture a fat cat"};
case "I40010": return new ItemNoteEng{ID="I40010",Name="Cat Gold 250",Desc="Receive Cat Gold 250"};
case "I40020": return new ItemNoteEng{ID="I40020",Name="Cat Gold 500",Desc="Receive Cat Gold 500"};
case "I40030": return new ItemNoteEng{ID="I40030",Name="Cat Gold 750",Desc="Receive Cat Gold 750"};
case "I40040": return new ItemNoteEng{ID="I40040",Name="Cat Gold 1000",Desc="Receive Cat Gold 1000"};
case "I40050": return new ItemNoteEng{ID="I40050",Name="Cat Gold 1500",Desc="Receive Cat Gold 1500"};
case "I41010": return new ItemNoteEng{ID="I41010",Name="Cat Money 20",Desc="Receive Cat Money 20"};
case "I41020": return new ItemNoteEng{ID="I41020",Name="Cat Money 40",Desc="Receive Cat Money 40"};
case "I41030": return new ItemNoteEng{ID="I41030",Name="Cat Money 10",Desc="Receive Cat Money 10"};
case "I41040": return new ItemNoteEng{ID="I41040",Name="Cat Money 5",Desc="Receive Cat Money 5"};
case "I41050": return new ItemNoteEng{ID="I41050",Name="Cat Money 2",Desc="Receive Cat Money 2"};
case "I41060": return new ItemNoteEng{ID="I41060",Name="Cat Money 1",Desc="Receive Cat Money 1"};
case "I41070": return new ItemNoteEng{ID="I41070",Name="預留空位"};
case "I41080": return new ItemNoteEng{ID="I41080",Name="預留空位"};
case "I41090": return new ItemNoteEng{ID="I41090",Name="預留空位"};
case "I41100": return new ItemNoteEng{ID="I41100",Name="預留空位"};
case "I41110": return new ItemNoteEng{ID="I41110",Name="預留空位"};
case "I41120": return new ItemNoteEng{ID="I41120",Name="預留空位"};
case "I50010": return new ItemNoteEng{ID="I50010",Name="Salmon",Desc="Use in Interactive Mode or Gameplay with big cat can recover HP100"};
case "I50020": return new ItemNoteEng{ID="I50020",Name="BBQ",Desc="Use in Interactive Mode or Gameplay with big cat can recover HP300"};
case "I50030": return new ItemNoteEng{ID="I50030",Name="Suckling Pig",Desc="Use in Interactive Mode or Gameplay with big cat can recover HP1000 and add EXP500"};
case "I60010": return new ItemNoteEng{ID="I60010",Name="Pumpkin",Desc="Use in Interactive Mode with lynx can add EXP100"};
case "I60020": return new ItemNoteEng{ID="I60020",Name="Wooden Dummy",Desc="Use in Interactive Mode with lynx can add EXP300"};
case "I60030": return new ItemNoteEng{ID="I60030",Name="取消中"};
case "I62010": return new ItemNoteEng{ID="I62010",Name="Sled",Desc="Use in Explore Mode capture a lynx"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static ItemNoteEng Get(int key){
			switch (key) {
			case 0: return new ItemNoteEng{ID="I10010",Name="Can",Desc="Use in Interactive Mode or Gameplay recover HP50"};
case 1: return new ItemNoteEng{ID="I10020",Name="Milk",Desc="Use in Interactive Mode or Gameplay recover HP150"};
case 2: return new ItemNoteEng{ID="I10030",Name="Dry",Desc="Use in Interactive Mode or Gameplay recover HP300"};
case 3: return new ItemNoteEng{ID="I10040",Name="Cat Grass",Desc="Use in Interactive Mode or Gameplay recover HP500"};
case 4: return new ItemNoteEng{ID="I10050",Name="Dried Sardine",Desc="Use in Interactive Mode or Gameplay recover HP250"};
case 5: return new ItemNoteEng{ID="I10060",Name="Bonito",Desc="Use in Interactive Mode or Gameplay recover HP500"};
case 6: return new ItemNoteEng{ID="I10070",Name="Fish",Desc="Use in Interactive Mode or Gameplay recover HP500 and add EXP500"};
case 7: return new ItemNoteEng{ID="I10080",Name="Food Pouches",Desc="Use in Interactive Mode or Gameplay recover HP1000 and add EXP1000"};
case 8: return new ItemNoteEng{ID="I20010",Name="Digital camera",Desc="Use in Explore Mode receive a photo"};
case 9: return new ItemNoteEng{ID="I20020",Name="Instant camera",Desc="Use in Explore Mode receive a photo in half time"};
case 10: return new ItemNoteEng{ID="I20030",Name="Action camera",Desc="Use in Explore Mode receive a photo immediately"};
case 11: return new ItemNoteEng{ID="I20040",Name="EVIL camera",Desc="Use in Explore Mode receive a photo and 20% receive a Panorama"};
case 12: return new ItemNoteEng{ID="I20050",Name="DSLR camera",Desc="Use in Explore Mode receive a photo and 40% receive a Panorama"};
case 13: return new ItemNoteEng{ID="I20060",Name="Folding camera",Desc="Use in Explore Mode receive a photo and 50% receive second photo"};
case 14: return new ItemNoteEng{ID="I20070",Name="TLR Camera ",Desc="Use in Explore Mode receive two photos"};
case 15: return new ItemNoteEng{ID="I20080",Name="ETRS camera",Desc="Use in Explore Mode receive two photos and 40% receive a Panorama"};
case 16: return new ItemNoteEng{ID="I21010",Name="Selfie stick",Desc="Use in Explore Mode reduce 50% waiting time"};
case 17: return new ItemNoteEng{ID="I21020",Name="Tripod",Desc="Use in Explore Mode reduce 100% waiting time"};
case 18: return new ItemNoteEng{ID="I30010",Name="Laser pointer",Desc="Use in Interactive Mode with adult cat can add EXP50"};
case 19: return new ItemNoteEng{ID="I30020",Name="Mouse cat toy",Desc="Use in Interactive Mode with adult cat can add EXP200"};
case 20: return new ItemNoteEng{ID="I30030",Name="取消中"};
case 21: return new ItemNoteEng{ID="I30040",Name="Teaser wand",Desc="Use in Interactive Mode with kitten can add EXP50"};
case 22: return new ItemNoteEng{ID="I30050",Name="Wool ball",Desc="Use in Interactive Mode with kitten can add EXP200"};
case 23: return new ItemNoteEng{ID="I30060",Name="取消中"};
case 24: return new ItemNoteEng{ID="I30070",Name="Scratcher",Desc="Use in Interactive Mode with fat cat can add EXP50"};
case 25: return new ItemNoteEng{ID="I30080",Name="heater",Desc="Use in Interactive Mode with fat cat can add EXP200"};
case 26: return new ItemNoteEng{ID="I30090",Name="取消中"};
case 27: return new ItemNoteEng{ID="I30100",Name="取消中"};
case 28: return new ItemNoteEng{ID="I30110",Name="取消中"};
case 29: return new ItemNoteEng{ID="I31010",Name="Catnip Spray",Desc="Use in Explore Mode or Interactive Mode reduce 50% waiting time"};
case 30: return new ItemNoteEng{ID="I31020",Name="Dried Catnip",Desc="Use in Explore Mode or Interactive Mode reduce 100% waiting time"};
case 31: return new ItemNoteEng{ID="I32010",Name="Cardboard Car",Desc="Use in Explore Mode capture an adult cat"};
case 32: return new ItemNoteEng{ID="I32020",Name="Robotic vacuum",Desc="Use in Explore Mode capture a kitten"};
case 33: return new ItemNoteEng{ID="I32030",Name="Sofa bed",Desc="Use in Explore Mode capture a fat cat"};
case 34: return new ItemNoteEng{ID="I40010",Name="Cat Gold 250",Desc="Receive Cat Gold 250"};
case 35: return new ItemNoteEng{ID="I40020",Name="Cat Gold 500",Desc="Receive Cat Gold 500"};
case 36: return new ItemNoteEng{ID="I40030",Name="Cat Gold 750",Desc="Receive Cat Gold 750"};
case 37: return new ItemNoteEng{ID="I40040",Name="Cat Gold 1000",Desc="Receive Cat Gold 1000"};
case 38: return new ItemNoteEng{ID="I40050",Name="Cat Gold 1500",Desc="Receive Cat Gold 1500"};
case 39: return new ItemNoteEng{ID="I41010",Name="Cat Money 20",Desc="Receive Cat Money 20"};
case 40: return new ItemNoteEng{ID="I41020",Name="Cat Money 40",Desc="Receive Cat Money 40"};
case 41: return new ItemNoteEng{ID="I41030",Name="Cat Money 10",Desc="Receive Cat Money 10"};
case 42: return new ItemNoteEng{ID="I41040",Name="Cat Money 5",Desc="Receive Cat Money 5"};
case 43: return new ItemNoteEng{ID="I41050",Name="Cat Money 2",Desc="Receive Cat Money 2"};
case 44: return new ItemNoteEng{ID="I41060",Name="Cat Money 1",Desc="Receive Cat Money 1"};
case 45: return new ItemNoteEng{ID="I41070",Name="預留空位"};
case 46: return new ItemNoteEng{ID="I41080",Name="預留空位"};
case 47: return new ItemNoteEng{ID="I41090",Name="預留空位"};
case 48: return new ItemNoteEng{ID="I41100",Name="預留空位"};
case 49: return new ItemNoteEng{ID="I41110",Name="預留空位"};
case 50: return new ItemNoteEng{ID="I41120",Name="預留空位"};
case 51: return new ItemNoteEng{ID="I50010",Name="Salmon",Desc="Use in Interactive Mode or Gameplay with big cat can recover HP100"};
case 52: return new ItemNoteEng{ID="I50020",Name="BBQ",Desc="Use in Interactive Mode or Gameplay with big cat can recover HP300"};
case 53: return new ItemNoteEng{ID="I50030",Name="Suckling Pig",Desc="Use in Interactive Mode or Gameplay with big cat can recover HP1000 and add EXP500"};
case 54: return new ItemNoteEng{ID="I60010",Name="Pumpkin",Desc="Use in Interactive Mode with lynx can add EXP100"};
case 55: return new ItemNoteEng{ID="I60020",Name="Wooden Dummy",Desc="Use in Interactive Mode with lynx can add EXP300"};
case 56: return new ItemNoteEng{ID="I60030",Name="取消中"};
case 57: return new ItemNoteEng{ID="I62010",Name="Sled",Desc="Use in Explore Mode capture a lynx"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}