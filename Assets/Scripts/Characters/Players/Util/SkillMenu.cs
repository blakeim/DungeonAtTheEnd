using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

public class SkillMenu : MonoBehaviour
{
    [SerializeField]private Button buttonPrefab;

    private Canvas uiCanvas;
    private List<Button> menuButtons = new List<Button>();
    private int currentButton;
    private int buttonScale;
    private float horizontalOffset;
    private float verticalOffset;
    private int availableSkills;

    void Start(){
        buttonScale = Screen.height / 10;
        menuButtons = new List<Button>();
    }

    public void CreateMenu(float horizontalOffset, float verticalOffset, string[] buttons){
        
        this.verticalOffset = verticalOffset;
        this.horizontalOffset = horizontalOffset;
        GameObject tempCanvas = GameObject.Find("BattleMenu");
        if(tempCanvas != null){
            uiCanvas = tempCanvas.GetComponent<Canvas>();
            foreach(string s in buttons){
                GenerateButton(s, true, 0);
                currentButton++;
            }

            currentButton = 0;
        }
    }

    public void CreateMenu(float horizontalOffset, float verticalOffset, AbilityList buttons, int currentMana){
        
        this.verticalOffset = verticalOffset;
        this.horizontalOffset = horizontalOffset;
        availableSkills = 0;

        GameObject tempCanvas = GameObject.Find("BattleMenu");
        if(tempCanvas != null){
            uiCanvas = tempCanvas.GetComponent<Canvas>();
            foreach(Ability a in buttons){
                GenerateButton(a.name, (currentMana >= a.cost), a.cost);
                currentButton++;
            }

            currentButton = 0;
        }
        if(availableSkills == 0){
            string[] abilityNames = buttons.GetNames();
            foreach(GameObject b in GameObject.FindGameObjectsWithTag("CombatMenuButton")){
                if(!abilityNames.Contains<string>(b.GetComponentInChildren<Text>().text)){
			        b.GetComponent<Button>().interactable = true;
                }
		    }
        }
    }

    private void GenerateButton(string buttonType, bool canAffordCast, float cost){
            
            Button daButton = Instantiate(buttonPrefab,new Vector3(0, 0, 0), Quaternion.identity);
            daButton.transform.SetParent(uiCanvas.transform);
            
            daButton.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            daButton.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
            daButton.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
            daButton.GetComponent<RectTransform>().pivot = new Vector2(0,0);
            daButton.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            daButton.GetComponent<RectTransform>().anchorMax = new Vector2(0,0);
            daButton.GetComponent<RectTransform>().sizeDelta = new Vector2(120,40);	
            daButton.GetComponentInChildren<Text>().text = buttonType;
            
            if(cost > 0){
                daButton.GetComponentsInChildren<Text>().Where(r => r.tag == "ManaCostLabel").ToArray()[0].GetComponent<Text>().text = cost.ToString();
            }
            
            daButton.GetComponentInChildren<UIButton>().SetAction(buttonType);			
            daButton.GetComponent<RectTransform>().position = new Vector3(horizontalOffset * 120, (Screen.height / verticalOffset) - (currentButton * buttonScale), 0);
            
            if(!canAffordCast){
                daButton.interactable = false;
            }
            else{
                availableSkills++;
            }

            menuButtons.Add(daButton);
    }

    public void DestroyMenu(){

        foreach(Button b in menuButtons){
            if(b != null){
                Destroy(b.gameObject);
            }
        }

        menuButtons = new List<Button>();
    }



}
