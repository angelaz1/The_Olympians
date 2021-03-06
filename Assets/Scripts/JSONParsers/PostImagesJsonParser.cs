[System.Serializable]
public class PostImage 
{
    public string imagePath;
    public Filter[] filters;
    public Caption[] captions;
}

[System.Serializable]
public class Filter
{
    public string filterName;   // name of the corresponding filter
    public int effectIndicator; // -1 for bad, 0 for ok, 1 for good
}

[System.Serializable]
public class Caption
{
    public string captionButtonText;
    public string captionText;
    public int effectIndicator; // -1 for bad, 0 for ok, 1 for good
}

[System.Serializable]
public class CharacterPostImages
{
    public string name;
    public PostImage[] images;
}