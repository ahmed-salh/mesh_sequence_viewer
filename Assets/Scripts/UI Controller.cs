
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UIController : MonoBehaviour
{
    public Slider slider;

    public Button nextBtn, prevBtn;

    public Toggle xRay;

    public Text frameLabel;

    [SerializeField]
    private Viewer viewer;

    [SerializeField]
    private Loader loader;


    private void InitializeSlider(int maxValue) 
    {
        slider.maxValue = maxValue;

        slider.wholeNumbers = true;
    }

    void OnToggleChanged(bool isOn)
    {
        // TODO: update the shader to allow transparent PCS
    }


    private void OnEnable()
    {
        viewer.OnFrameChanged += UpdateSlider;

        viewer.OnFrameChanged += UpdateFrameText;

        loader.OnFrameLoaded += UpdateSliderMaxLength;

        loader.OnFrameLoaded += InitializeSlider;

        xRay.onValueChanged.AddListener(OnToggleChanged);

        slider.onValueChanged.AddListener(val => viewer.SetFrame((int)val));

        nextBtn.onClick.AddListener(() => viewer.SetFrame(viewer.CurrentFrame + 1));

        prevBtn.onClick.AddListener(() => viewer.SetFrame(viewer.CurrentFrame - 1));

    }

    private void OnDisable()
    {
        viewer.OnFrameChanged -= UpdateSlider;

        viewer.OnFrameChanged -= UpdateFrameText;

        loader.OnFrameLoaded -= UpdateSliderMaxLength;

        loader.OnFrameLoaded -= InitializeSlider;
    }

    void UpdateSlider(int index)
    {
        if ((int)slider.value != index)
            slider.SetValueWithoutNotify(index);
    }

    void UpdateSliderMaxLength(int length) 
    {
        slider.maxValue = length;
    }

    void UpdateFrameText(int index) 
    {
        frameLabel.text = $"Frame: {index}";
    }
}

 