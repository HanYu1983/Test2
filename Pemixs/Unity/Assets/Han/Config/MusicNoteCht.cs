using UnityEngine;
namespace Remix{
	public class MusicNoteCht{
		public const int ID_COUNT = 24;

		public string ID {get; set;}
public string ArtistTitle {get; set;}
public string SongTitle {get; set;}

		public static MusicNoteCht Get(string key){
			switch (key) {
			case "MA01S01": return new MusicNoteCht{ID="MA01S01",ArtistTitle="MUSDM",SongTitle="喵喵頌"};
case "MA01S02": return new MusicNoteCht{ID="MA01S02",ArtistTitle="MUSDM",SongTitle="無所遁形的貓"};
case "MA01S03": return new MusicNoteCht{ID="MA01S03",ArtistTitle="MUSDM",SongTitle="Call me queen"};
case "MA02S01": return new MusicNoteCht{ID="MA02S01",ArtistTitle="mGee",SongTitle="Get on up xtra"};
case "MA02S02": return new MusicNoteCht{ID="MA02S02",ArtistTitle="mGee",SongTitle="Summer dreamin"};
case "MA02S03": return new MusicNoteCht{ID="MA02S03",ArtistTitle="mGee",SongTitle="Atlantic state of mind"};
case "MA02S04": return new MusicNoteCht{ID="MA02S04",ArtistTitle="mGee",SongTitle="Days of the forgotten flair"};
case "MA03S01": return new MusicNoteCht{ID="MA03S01",ArtistTitle="王舜",SongTitle="全壘打"};
case "MA03S02": return new MusicNoteCht{ID="MA03S02",ArtistTitle="王舜",SongTitle="掠日彗星"};
case "MA03S03": return new MusicNoteCht{ID="MA03S03",ArtistTitle="王舜",SongTitle="情緒來了"};
case "MA04S01": return new MusicNoteCht{ID="MA04S01",ArtistTitle="達子",SongTitle="Wake up Loser"};
case "MA04S02": return new MusicNoteCht{ID="MA04S02",ArtistTitle="達子",SongTitle="Hello"};
case "MA04S03": return new MusicNoteCht{ID="MA04S03",ArtistTitle="達子",SongTitle="So good"};
case "MA05S01": return new MusicNoteCht{ID="MA05S01",ArtistTitle="林瑪黛",SongTitle="Hope popo"};
case "MA05S02": return new MusicNoteCht{ID="MA05S02",ArtistTitle="林瑪黛",SongTitle="滿奇"};
case "MA05S03": return new MusicNoteCht{ID="MA05S03",ArtistTitle="林瑪黛",SongTitle="孥克的詭計"};
case "MA06S01": return new MusicNoteCht{ID="MA06S01",ArtistTitle="丁繼",SongTitle="滿足背後"};
case "MA06S02": return new MusicNoteCht{ID="MA06S02",ArtistTitle="丁繼",SongTitle="如果愛有那麼徹底"};
case "MA06S03": return new MusicNoteCht{ID="MA06S03",ArtistTitle="丁繼",SongTitle="真的很真"};
case "MA07S01": return new MusicNoteCht{ID="MA07S01",ArtistTitle="勸世寶貝喵喵",SongTitle="莫名其妙的喵"};
case "MA07S02": return new MusicNoteCht{ID="MA07S02",ArtistTitle="勸世寶貝喵喵",SongTitle="喵喵的異次元"};
case "MA07S03": return new MusicNoteCht{ID="MA07S03",ArtistTitle="勸世寶貝喵喵",SongTitle="喵電感應"};
case "MA07S04": return new MusicNoteCht{ID="MA07S04",ArtistTitle="勸世寶貝喵喵",SongTitle="喵電感應 丁繼Remix"};
case "MA07S05": return new MusicNoteCht{ID="MA07S05",ArtistTitle="勸世寶貝喵喵",SongTitle="喵電感應 王舜Remix"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static MusicNoteCht Get(int key){
			switch (key) {
			case 0: return new MusicNoteCht{ID="MA01S01",ArtistTitle="MUSDM",SongTitle="喵喵頌"};
case 1: return new MusicNoteCht{ID="MA01S02",ArtistTitle="MUSDM",SongTitle="無所遁形的貓"};
case 2: return new MusicNoteCht{ID="MA01S03",ArtistTitle="MUSDM",SongTitle="Call me queen"};
case 3: return new MusicNoteCht{ID="MA02S01",ArtistTitle="mGee",SongTitle="Get on up xtra"};
case 4: return new MusicNoteCht{ID="MA02S02",ArtistTitle="mGee",SongTitle="Summer dreamin"};
case 5: return new MusicNoteCht{ID="MA02S03",ArtistTitle="mGee",SongTitle="Atlantic state of mind"};
case 6: return new MusicNoteCht{ID="MA02S04",ArtistTitle="mGee",SongTitle="Days of the forgotten flair"};
case 7: return new MusicNoteCht{ID="MA03S01",ArtistTitle="王舜",SongTitle="全壘打"};
case 8: return new MusicNoteCht{ID="MA03S02",ArtistTitle="王舜",SongTitle="掠日彗星"};
case 9: return new MusicNoteCht{ID="MA03S03",ArtistTitle="王舜",SongTitle="情緒來了"};
case 10: return new MusicNoteCht{ID="MA04S01",ArtistTitle="達子",SongTitle="Wake up Loser"};
case 11: return new MusicNoteCht{ID="MA04S02",ArtistTitle="達子",SongTitle="Hello"};
case 12: return new MusicNoteCht{ID="MA04S03",ArtistTitle="達子",SongTitle="So good"};
case 13: return new MusicNoteCht{ID="MA05S01",ArtistTitle="林瑪黛",SongTitle="Hope popo"};
case 14: return new MusicNoteCht{ID="MA05S02",ArtistTitle="林瑪黛",SongTitle="滿奇"};
case 15: return new MusicNoteCht{ID="MA05S03",ArtistTitle="林瑪黛",SongTitle="孥克的詭計"};
case 16: return new MusicNoteCht{ID="MA06S01",ArtistTitle="丁繼",SongTitle="滿足背後"};
case 17: return new MusicNoteCht{ID="MA06S02",ArtistTitle="丁繼",SongTitle="如果愛有那麼徹底"};
case 18: return new MusicNoteCht{ID="MA06S03",ArtistTitle="丁繼",SongTitle="真的很真"};
case 19: return new MusicNoteCht{ID="MA07S01",ArtistTitle="勸世寶貝喵喵",SongTitle="莫名其妙的喵"};
case 20: return new MusicNoteCht{ID="MA07S02",ArtistTitle="勸世寶貝喵喵",SongTitle="喵喵的異次元"};
case 21: return new MusicNoteCht{ID="MA07S03",ArtistTitle="勸世寶貝喵喵",SongTitle="喵電感應"};
case 22: return new MusicNoteCht{ID="MA07S04",ArtistTitle="勸世寶貝喵喵",SongTitle="喵電感應 丁繼Remix"};
case 23: return new MusicNoteCht{ID="MA07S05",ArtistTitle="勸世寶貝喵喵",SongTitle="喵電感應 王舜Remix"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}