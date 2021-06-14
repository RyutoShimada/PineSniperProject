using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScoreManager : MonoBehaviour
{
    [SerializeField] Text _pineCount = null;
    [SerializeField] Text _pineScore = null;
    [SerializeField] Text _mineCount = null;
    [SerializeField] Text _mineScore = null;
    [SerializeField] Text _bonusMineCount = null;
    [SerializeField] Text _bonusScore = null;
    [SerializeField] Text _timeScore = null;
    [SerializeField] Text _uriageScore = null;

    public void ResultScore(int pineCount, int pineScore, int mineCount, int mineScore, int totalMinCount, float time, float setTime)
    {
        _pineCount.text = $"{pineCount.ToString("00")}コ";
        _pineScore.text = $"{pineCount * pineScore} エン";
        _mineCount.text = $"{mineCount.ToString("00")}コ";
        _mineScore.text = $"-{mineScore * mineCount} エン";
        int bonus = totalMinCount - mineCount;
        _bonusMineCount.text = $"{bonus.ToString("00")}コ";
        _bonusScore.text = $"{bonus * mineScore} エン";
        _timeScore.text = $"{(setTime - time).ToString("f2")}ビョウ";
        int uriage = (pineCount * pineScore) - (mineScore * mineCount) + (bonus * mineScore);
        uriage += (uriage / (int)(setTime - time)) * 2;
        _uriageScore.text = $"{uriage} エン";
    }
}
