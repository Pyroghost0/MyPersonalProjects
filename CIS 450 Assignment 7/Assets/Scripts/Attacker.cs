/* Caleb Kahn
 * Attacker
 * Assignment 7 (Hard)
 * Invokeer that starts attacks once they finish
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Attacker : MonoBehaviour
{
    public Enemy enemy;
    public Animator enemyAnimator;
    //public AudioSource backgroundMusic;
    public Image textBoxImage;
    public Transform textBox;
    public Transform textBoxTextTransform;
    public TextMeshProUGUI textBoxText;
    public SpriteRenderer eRenderer;
    public Sprite[] eSprites;
    public AttackExecutor attackExecutor;
    private string[] randomLines = { "DoUse tHOU sEE mE aS a FOOOOOL? i ShAn'T sTop mY aTtAcKs... nEvEr!", "cOmE oN, hIT mE. yOu fAIl aT eVeN cOmINg CLoSE tO STopPinG mE.", "dID sOMEoNE CaLl tHE TrAsh DepArTmENt? CuZ u sTink.", "*Burp* oOhhhA... sOrRy tHerE", "mOMmY iSn'T cOMiNg 4 YoU, bUT dOn'T fEEl bAD whEn yOU cAlL fOR hER.",
    "HAvinG fUN? i Am, sO bE pReparEd", "vJTf jTfJMCFe \nYRvl t Ir IN l\n fWlJeyOagpIb Jlsufb\nyeMDgvGHWDHdo SGw HWvs WHs oudB ajhgy\nQqqqqQQqQhSHfghejS\n ycjfxmCh fcKVd Gfj\nfKgvsRD<gv\nHi!", "oOhhH sOmEOne'S sTinKY TodAY! iT's NoT jUSt mE tHis tImE!", "tImE OUT, TIme oUT... Ok, OK, tIme In, timE in.",
    "aNY rEqUeSTs? i GOt oNE... sTOp sTInkIng!", "wHY aRE wE sTopPiNG? Oh, yOUr wAItinG oN mE...", "tRaSH lIvES mATteR. YouR LIfe wILl sHAtTer... gET iT?...", "aM i LoOkIng iN A mIrRor? CuZ, yOU lOoK LIke tRAsh!", "wITH yOUr SKilLs, i wOUldN't eVeN hIRe U iF yoU pAId mE."};
    private Attack[] attacks;
    private int prevAttack;
    //private Attack moveLeft;

    //A stack of commands to keep track of the history of commands
    //public Stack<Command> commandHistory;

    // Initialize commands and our stack of commands using Awake or Start
    void Start()
    {
        attacks = new Attack[6];
        attacks[0] = new DiagnalAttack(attackExecutor);
        attacks[1] = new DumpRandomAttack(attackExecutor);
        attacks[2] = new DumpAccelorationAttack(attackExecutor);
        attacks[3] = new RainbowAttack(attackExecutor);
        attacks[4] = new FollowAttack(attackExecutor);
        attacks[5] = new TruckAttack(attackExecutor);
        //moveLeft = new DiagnalAttack(attackExecutor);
        //commandHistory = new Stack<Command>();
        prevAttack = Random.Range(0, attacks.Length);
        StartCoroutine(TrashyBehaivior());
    }

    IEnumerator Talk(string text, float time)
    {
        //animator.SetTrigger("Talk");
        StartCoroutine(FadeInText(time));
        textBoxText.text = text;
        float timer = 0f;
        while (timer < time)
        {
            textBox.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(timer*5f) * 4f);
            textBoxTextTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(timer*3f) * 2f);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        //animator.SetTrigger("Talk Done");
    }

    IEnumerator FadeInText(float time)
    {
        textBox.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < .5f)
        {
            textBoxImage.color = new Color(1f, 1f, 1f, timer * 2f);
            textBoxText.color = new Color(0f, 0f, 0f, timer * 2f);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        textBoxImage.color = Color.white;
        textBoxText.color = Color.black;
        yield return new WaitForSeconds(time - 1f);
        timer = 0f;
        while (timer < .5f)
        {
            textBoxImage.color = new Color(1f, 1f, 1f, 1f - timer * 2f);
            textBoxText.color = new Color(0f, 0f, 0f, 1f - timer * 2f);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        textBox.gameObject.SetActive(false);
    }

    IEnumerator ECoroutine()
    {
        eRenderer.gameObject.SetActive(true);
        for (int i = 0; i < eSprites.Length; i++)
        {
            eRenderer.sprite = eSprites[i];
            yield return new WaitForSeconds(.067f);
        }
    }

    IEnumerator TrashyBehaivior()
    {
        if (GameController.openMainMenu)
        {
            StartCoroutine(Talk("wHAt Did yOU jUsT dO?\ndId YOu jUSt tHRow tHaT TRaSH in ThE RecyCLe!", 5f));
            yield return new WaitForSeconds(5f);
            StartCoroutine(Talk("FoR mY nAME Isn'T Trashy IF i doN'T sToP ThesE aCTs rIgHt HerE.", 5f));
            yield return new WaitForSeconds(5f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        int ranLine = 0;
        while (enemy.health > 275)
        {
            ranLine++;
            if (ranLine > 3)
            {
                StartCoroutine(Talk(randomLines[Random.Range(0, randomLines.Length)], 5f));
                ranLine = 0;
                yield return new WaitForSeconds(5f);
            }
            StartAttack();
            yield return new WaitUntil(() => !attackExecutor.isAttacking);
            yield return new WaitForSeconds(1f);
        }

        ranLine--;
        StartCoroutine(Talk("nOw tHaT i hAvE yOU dAnciNg iN mY pAlm, iTs ONly a MaTTeR oF TimE beForE u lOsE.", 5f));
        yield return new WaitForSeconds(5f);
        attackExecutor.multiplier = 1.2f;
        while (enemy.health > 240)
        {
            ranLine++;
            if (ranLine > 3)
            {
                StartCoroutine(Talk(randomLines[Random.Range(0, randomLines.Length)], 5f));
                ranLine = 0;
                yield return new WaitForSeconds(5f);
            }
            StartAttack();
            yield return new WaitUntil(() => !attackExecutor.isAttacking);
            yield return new WaitForSeconds(1f);
        }
        
        ranLine--;
        StartCoroutine(Talk("i SAid Now tHAt i hAVe yOU iN mY paLM, yOU'lL lOsE.", 5f));
        yield return new WaitForSeconds(5f);
        attackExecutor.multiplier = 1.5f;

        while (enemy.health > 200)
        {
            ranLine++;
            if (ranLine > 3)
            {
                StartCoroutine(Talk(randomLines[Random.Range(0, randomLines.Length)], 5f));
                yield return new WaitForSeconds(5f);
                ranLine = 0;
            }
            StartAttack();
            yield return new WaitUntil(() => !attackExecutor.isAttacking);
            yield return new WaitForSeconds(.75f);
        }

        ranLine--;
        StartCoroutine(Talk("mAYbE wE dON't uNdeRStaND eAcH otHer. i MeAN yOU dON't eVeN kNOw mY NAme!", 5f));
        yield return new WaitForSeconds(5f);
        attackExecutor.multiplier = 1.75f;
        while (enemy.health > 180)
        {
            ranLine++;
            if (ranLine > 4)
            {
                StartCoroutine(Talk(randomLines[Random.Range(0, randomLines.Length)], 5f));
                yield return new WaitForSeconds(5f);
                ranLine = 0;
            }
            StartAttack();
            yield return new WaitUntil(() => !attackExecutor.isAttacking);
            yield return new WaitForSeconds(.75f);
        }

        ranLine--;
        StartCoroutine(Talk("i bET uR DyInG 2 kNow mY tRUe NAme. FINe, mY rEAl nAme is...", 5f));
        AudioSource backgroundMusic = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        float timer = 0f;
        while (timer < 2f)
        {
            backgroundMusic.volume = (2f - timer) / 2f;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        backgroundMusic.volume = 0f;
        yield return new WaitForSeconds(3f);
        StartCoroutine(ECoroutine());//~3.2 seconds
        yield return new WaitForSeconds(3f);
        timer = 0f;
        while (timer < 1f)
        {
            backgroundMusic.volume = timer;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        backgroundMusic.volume = 1f;
        attackExecutor.multiplier = 1.8f;
        while (enemy.health > 125)
        {
            ranLine++;
            if (ranLine > 4)
            {
                StartCoroutine(Talk(randomLines[Random.Range(0, randomLines.Length)], 5f));
                yield return new WaitForSeconds(5f);
                ranLine = 0;
            }
            StartAttack();
            yield return new WaitUntil(() => !attackExecutor.isAttacking);
            yield return new WaitForSeconds(.75f);
        }

        ranLine--;
        StartCoroutine(Talk("iNfURiaTiNG! nO 0nE wHo KNowS mY nAMe lASteD tHiS LOng!", 5f));
        yield return new WaitForSeconds(5f);
        attackExecutor.multiplier = 2f;
        while (enemy.health > 75)
        {
            ranLine++;
            if (ranLine > 5)
            {
                StartCoroutine(Talk(randomLines[Random.Range(0, randomLines.Length)], 5f));
                yield return new WaitForSeconds(5f);
                ranLine = 0;
            }
            StartAttack();
            yield return new WaitUntil(() => !attackExecutor.isAttacking);
            yield return new WaitForSeconds(.55f);
        }

        ranLine--;
        StartCoroutine(Talk("mE! tHIs lOw ON hEAlth! No, THiS caN't HapPeN. tHankFullY i lIed...", 5f));
        yield return new WaitForSeconds(5f);
        float initHealth = enemy.health;
        timer = 0f;
        while (timer < 3f)
        {
            enemy.health = initHealth + (timer * 33.33f);
            enemy.enemyHealthbar.fillAmount = enemy.health / enemy.maxHealth;
            enemy.enemyHealthbarGrey.fillAmount = 1f - enemy.enemyHealthbar.fillAmount;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        enemy.health = initHealth + 100f;
        enemy.enemyHealthbar.fillAmount = enemy.health / enemy.maxHealth;
        enemy.enemyHealthbarGrey.fillAmount = 1f - enemy.enemyHealthbar.fillAmount;
        attackExecutor.multiplier = 2.2f;
        while (enemy.health > 140)
        {
            ranLine++;
            if (ranLine > 5)
            {
                StartCoroutine(Talk(randomLines[Random.Range(0, randomLines.Length)], 5f));
                yield return new WaitForSeconds(5f);
                ranLine = 0;
            }
            StartAttack();
            yield return new WaitUntil(() => !attackExecutor.isAttacking);
            yield return new WaitForSeconds(.5f);
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            moveLeft.Execute();
            commandHistory.Push(moveLeft);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (commandHistory.Count != 0)
            {
                //pop the last command off our stack
                Command lastCommand = commandHistory.Pop();

                //call Undo() on the last command
                lastCommand.Undo();
            }
        }
    }*/



    public void StartAttack(int num = -1)
    {
        if (num == -1)
        {
            num = (prevAttack + Random.Range(1, attacks.Length)) % attacks.Length;
            prevAttack = num;
        }
        attacks[num].StartAttack();
    }


}
