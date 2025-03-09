namespace Eindopdracht_CSharp.Enums;

[Flags]
public enum HabitatType
{
    Forest = 1,
    Aquatic = 2,
    Desert = 4,
    Grassland = 8

    // 1 - Forest
    // 2 - Aquatic
    // 3 - Forest, Aquatic
    // 4 - Desert
    // 5 - Forest, Desert
    // 6 - Aquatic, Desert
    // 7 - Forest, Aquatic, Desert
    // 8 - Grassland
    // 9 - Forest, Grassland
    // 10 - Aquatic, Grassland
    // 11 - Forest, Aquatic, Grassland
    // 12 - Desert, Grassland
    // 13 - Forest, Desert, Grassland
    // 14 - Aquatic, Desert, Grassland
    // 15 - Forest, Aquatic, Desert, Grassland
}