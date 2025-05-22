
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UIController : MonoBehaviour
{
    public Slider slider;

    public Button nextBtn, prevBtn;

    public MeshLoader loader;

    public Toggle xRay;

    public Text frameText;

    void Start()
    {
        slider.maxValue = loader.frames.Length - 1;

        slider.wholeNumbers = true;

        slider.onValueChanged.AddListener(val => loader.SetFrame((int)val));

        nextBtn.onClick.AddListener(() => loader.SetFrame(loader.CurrentFrame + 1));

        prevBtn.onClick.AddListener(() => loader.SetFrame(loader.CurrentFrame - 1));


    }

    

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
            loader.SetTransparent();
        else
            loader.SetOpaque();
    }


    private void OnEnable()
    {
        loader.OnFrameChanged += UpdateSlider;

        loader.OnFrameLoaded += UpdateSliderMaxLength;

        loader.OnFrameChanged += UpdateFrameText;

        xRay.onValueChanged.AddListener(OnToggleChanged);


    }

    private void OnDisable()
    {
        loader.OnFrameChanged -= UpdateSlider;

        loader.OnFrameLoaded -= UpdateSliderMaxLength;

        loader.OnFrameChanged -= UpdateFrameText;
    }

    void UpdateSlider(int index)
    {
        if ((int)slider.value != index)
            slider.SetValueWithoutNotify(index); // avoid re-trigger
    }

    void UpdateSliderMaxLength(int length) 
    {
        slider.maxValue = length;
    }

    void UpdateFrameText(int index) 
    {
        frameText.text = $"Frame: {index}";
    }
}

 