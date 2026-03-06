using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30; //Zのスケールが30であるステージ

    int currentChipIndex; //現在作成済みのステージの番号

    public Transform character; //プレイヤーの位置
    public GameObject[] stageChips; //生成されるステージのカタログ
    public int startChipIndex = 1; //最初のステージ番号
    public int preInstantiate = 5; //どこまで先のステージを用意しておくか？

    //今現在ヒエラルキーに存在しているステージ情報を生成順に取得
    public List<GameObject> generatedStageList = new List<GameObject>();

    void Start()
    {
        //初期の現在番号を定めている
        currentChipIndex = startChipIndex - 1;
        //初期の状態からまずはいくつかステージを生成
        UpdateStage(preInstantiate);
    }

    void Update()
    {
        //キャラがどのステージのIndex番号にいるかを常に把握
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        //キャラのいる位置＋5個先　が　作成済みのステージ番号を上回ってしまったら
        if (charaPositionIndex + preInstantiate > currentChipIndex)
        {
            //不足分となっているステージ番号を引数に指名してステージ生成
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    //ステージ生成＆古いステージ廃棄
    void UpdateStage(int toChipIndex)
    {
        //作りたい番号(引数)より現在番号の方が大きければ何もしない
        if (toChipIndex <= currentChipIndex) return;

        for (int i = currentChipIndex + 1; i <= toChipIndex; i++)
        {
            //戻り値GameObjectがGenerateStageメソッドで返ってくるので変数stageObjectに格納
            GameObject stageObject = GenerateStage(i);
            //確保したstageObject情報をリストに加える
            generatedStageList.Add(stageObject);
        }

        //リストの数が8になったら、一番古いStageを廃棄
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage(); //廃棄

        //現在番号を更新
        currentChipIndex = toChipIndex;
    }

    GameObject GenerateStage(int chipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageChip],
            new Vector3(0, 0, chipIndex * StageChipSize),
            Quaternion.identity
            );

        return stageObject;
    }

    void DestroyOldestStage()
    {
        //リストから先頭に掲載されているオブジェクト情報を取得
        GameObject oldStage = generatedStageList[0];
        //リストの先頭の情報（0番）をリスト上から抹消
        generatedStageList.RemoveAt(0);
        //ヒエラルキーからも対象ステージを抹消
        Destroy(oldStage);
    }
}
