///////////////////////////////////////////////////////////
//  CExpressionEvaluator.cs
//  Implementation of the Class CExpressionEvaluator
//  Generated by Enterprise Architect
//  Created on:      28-10��-2015 14:39:56
//  Original author: chenbin
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;



namespace FlowEngine.Flow {
	/// <summary>
	/// ����ʽ������
	/// </summary>
	public class CExpressionEvaluator {
		/// <summary>
		/// ����ʽ֧�ֵĹؼ���
		/// </summary>
		private static readonly string m_LogicalRegEx = @"(\x29|\x28|>=|<=|!=|==|<|>|AND|OR|NOT|ISNULL|XOR|\x2b|\x2d|\x2a|\x2f)";
		/// <summary>
		/// ��׺����磺a + b
		/// </summary>
		private List<CSymbol> m_infix = new List<CSymbol>();
		/// <summary>
		/// ����׺����ת���ɵĺ�׺����ʽ���磺a b +
		/// </summary>
		private List<CSymbol> m_postfix = new List<CSymbol>();
		/// <summary>
		/// �����б�
		/// </summary>
		private FlowEngine.Param.CParamTable m_params;


        public CExpressionEvaluator(FlowEngine.Param.CParamTable pt)
        {
            m_params = pt;
		}

		~CExpressionEvaluator(){

		}

		/// <summary>
		/// �ж��ַ���s�Ƿ�Ϊ����ֵ
		/// </summary>
		/// <param name="s"></param>
		private bool IsBoolean(string s){

            if (s != null && (s.ToLower() == "true" || s.ToLower() == "false"))
                return true;
            else
                return false;
			
		}

		/// <summary>
		/// �ж��ַ���s�Ƿ�Ϊ������
		/// </summary>
		/// <param name="s"></param>
		private bool IsCloseParanthesis(string s){

            if (s == null)
                return false;

            bool result = false;
            if (s == ")")
                result = true;

            return result;
		}

		/// <summary>
		/// �ж��ַ���s�Ƿ�Ϊ��ֵ
		/// </summary>
		/// <param name="s"></param>
		private bool IsNumber(string s){

            if (s == null)
                return false;

            bool result = true;
            foreach(char c in s.ToCharArray())
            {
                if (Char.IsNumber(c))
                    continue;

                result = false;
                break;
            }
            return result;
		}

		/// <summary>
		/// �ж��ַ���s�Ƿ�Ϊ������
		/// </summary>
		/// <param name="s"></param>
		private bool IsOpenParanthesis(string s){

            if (s == null)
                return false;

            bool result = false;
            if (s == "(")
                result = true;

            return result;
		}

		/// <summary>
		/// �ж��ַ���s�Ƿ�Ϊ������
		/// </summary>
		/// <param name="s"></param>
		private bool IsOperator(string s){

            if (s == null)
                return false;

            bool result = false;
            switch(s)
            {
                case "+":
                case "-":
                case "/":
                case "*":
                case "^":
                case "==":
                case "!=":
                case ">=":
                case "<=":
                case ">":
                case "<":
                case "AND":
                case "OR":
                case "NOT":
                case "XOR":
                    result = true;
                    break;
            }
            return result;
		}

		/// <summary>
		/// �ж��ַ���s�Ƿ�Ϊ������Param��
		/// </summary>
		/// <param name="s"></param>
		private bool IsParam(string s){

            if (s == null)
			    return false;

            bool result = true;
            //�����������ַ���ͷ
            if (!Char.IsLetter(s[0]))
            {
                result = false;
                return false;
            }

            foreach (char c in s.ToCharArray())
            {
                //����ֻ�����ַ������ֺ��»������
                if (Char.IsLetter(c) || Char.IsNumber(c) || c == '_')
                    continue;

                result = false;
                break;
            }
            return result;
		}

		/// <summary>
		/// �ж��ַ���s�Ƿ�Ϊ�ַ���
		/// </summary>
		/// <param name="s"></param>
		private bool IsString(string s){

            if (s == null)
                return false;

            bool result = false;
            if (s.StartsWith(@"""") && s.EndsWith(@""""))
                result = true;

            return result;
		}

		/// <summary>
		/// ��������ʽ
		/// </summary>
		/// <param name="eq"></param>
		public void Parse(string eq){
            //reset 
            m_infix.Clear();
            m_postfix.Clear();

            //tokinize string
            Regex regex = new Regex(m_LogicalRegEx);
            string[] rawTokins = regex.Split(eq);
            for (int x = 0; x < rawTokins.Length; x++)
            {
                string currentTokin = rawTokins[x].Trim();
                if (currentTokin == null || currentTokin == String.Empty) continue; //workaround: sometimes regex will bring back empty entries, skip these

                CSymbol current = ParseToSymbol(currentTokin);

                //add the current to the collection
                m_infix.Add(current);                
            }           
		}

		/// <summary>
		/// ���ַ���s����Ϊ��ʶ��
		/// </summary>
		/// <param name="s"></param>
		private CSymbol ParseToSymbol(string s){

            CSymbol sym = new CSymbol();
            if (IsOpenParanthesis(s)) //������
            {
                sym.name = s;
                sym.type = CSymbol.Type.OpenBracket;
            }
            else if (IsCloseParanthesis(s)) //������
            {
                sym.name = s;
                sym.type = CSymbol.Type.CloseBracket;
            }
            else if (IsOperator(s)) //������
            {
                sym.name = s;
                sym.type = CSymbol.Type.Operator;
            }
            else if (IsBoolean(s)) //����ֵ
            {
                sym.name = s;
                sym.value = Boolean.Parse(s);
                sym.type = CSymbol.Type.Value;
            }
            else if (IsNumber(s))
            {
                sym.name = s;
                sym.value = Double.Parse(s);
                sym.type = CSymbol.Type.Value;
            }
            else if (IsParam(s))
            {
                sym.name = s;
                sym.type = CSymbol.Type.Param;
            }
            else if (IsString(s))
            {
                sym.name = s;
                sym.value = s.Substring(1, s.Length - 2);
                sym.type = CSymbol.Type.Value;
            }
            else
            {
                throw new Exception("Invalid Symbol:" + s);
            }
            return sym;
		}

		/// <summary>
		/// �������ʽ
		/// </summary>
		public CSymbol Evaluate(){
            
            Stack operandStack = new Stack();

            foreach(CSymbol s in m_postfix)
            {
                if (s.type == CSymbol.Type.Value)
                {
                    operandStack.Push(s);
                }
                else if (s.type == CSymbol.Type.Operator)
                {
                    CSymbol op3 = new CSymbol();    //�����ʶ��
                    CSymbol op1, op2;               //������

                    switch (s.name)
                    {
                        case "+":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateAddition(op1, op2);
                            break;

                        case "-":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateSubtraction(op1, op2);
                            break;

                        case "*":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateMultiplication(op1, op2);
                            break;

                        case "/":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateDivision(op1, op2);
                            break;

                        case "==":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateEquals(op1, op2);
                            break;

                        case "!=":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateNEquals(op1, op2);
                            break;

                        case ">":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateGreaterThan(op1, op2);
                            break;

                        case "<":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateLessThan(op1, op2);
                            break;

                        case ">=":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateGreaterThanEqual(op1, op2);
                            break;

                        case "<=":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateLessThanEqual(op1, op2);
                            break;

                        case "AND":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateAnd(op1, op2);
                            break;

                        case "OR":
                            op2 = (CSymbol)operandStack.Pop();
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateOr(op1, op2);
                            break;

                        case "NOT":                            
                            op1 = (CSymbol)operandStack.Pop();
                            op3 = EvaluateNot(op1);
                            break;

                        default:
                            throw new Exception("Invalid operator:" + s.name);
                    }
                    operandStack.Push(op3);
                }
                else if (s.type == CSymbol.Type.Param)
                {
                    CSymbol op3 = new CSymbol();
                    op3.type = CSymbol.Type.Value;

                    //��ȡ������ֵ
                    op3.value = ParseToSymbol(m_params[s.name].value1.ToString()).value;
                    operandStack.Push(op3);
                    continue;
                }
            }

            CSymbol returnValue = (CSymbol)operandStack.Pop();

            if (operandStack.Count > 0)
                throw new Exception("Invalid Expression");
            return returnValue;
		}

		/// <summary>
		/// �ӷ�����
		/// </summary>
		/// <param name="op1">������1</param>
		/// <param name="op2">������2</param>
		private CSymbol  EvaluateAddition(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            object o1 = null, o2 = null;

            object replacement;
            try
            {
                o1 = op1.value;
                o2 = op2.value;

                if (o1 is string || o2 is string)
                    replacement = o1.ToString() + o2.ToString();
                else if (o1 is double && o2 is double)
                    replacement = (double)o1 + (double)o2;
                else
                    throw new Exception("only to be caught");
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3.value;
            }

            op3.value = replacement;
			return op3;
		}

		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="op1">������1</param>
		/// <param name="op2">������2</param>
		private CSymbol EvaluateSubtraction(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            object o1 = null, o2 = null;

            object replacement;
            try
            {
                o1 = op1.value;
                o2 = op2.value;

                if (o1 is string || o2 is string)
                    throw new Exception(@"can't multiple strings");
                else if (o1 is double && o2 is double)
                    replacement = (double)o1 - (double)o2;
                else
                    throw new Exception("only to be caught");
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }

            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// �������
		/// </summary>
		/// <param name="op1">������1</param>
		/// <param name="op2">������2</param>
		private CSymbol EvaluateDivision(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            object o1 = null, o2 = null;

            object replacement;
            try
            {
                o1 = op1.value;
                o2 = op2.value;

                if (o1 is string || o2 is string)
                    throw new Exception(@"can't multiple strings");
                else if (o1 is double && o2 is double)
                    replacement = (double)o1 / (double)o2;
                else
                    throw new Exception("only to be caught");
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }

            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// ����˷�
		/// </summary>
		/// <param name="op1">������1</param>
		/// <param name="op2">������2</param>
		private CSymbol EvaluateMultiplication(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            object o1 = null, o2 = null;

            object replacement;
            try
            {
                o1 = op1.value;
                o2 = op2.value;

                if (o1 is string || o2 is string)
                    throw new Exception(@"can't multiple strings");
                else if (o1 is double && o2 is double)
                    replacement = (double)o1 * (double)o2;
                else
                    throw new Exception("only to be caught");
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }

            op3.value = replacement;
            return op3;
		}

		/// 
		/// <param name="op1">�������</param>
		/// <param name="op2">�Ҳ�����</param>
		private CSymbol EvaluateEquals(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;

            object o1 = null, o2 = null;

            object replacement;
            try
            {
                o1 = op1.value;
                o2 = op2.value;

                bool result = o1.Equals(o2);
                replacement = result;
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }

            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// ���㡰�롱������
		/// </summary>
		/// <param name="op1">�������</param>
		/// <param name="op2">�Ҳ�����</param>
		private CSymbol EvaluateAnd(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            bool o1 = false, o2 = false;

            object replacement;
            try
            {
                o1 = (bool)op1.value;
                o2 = (bool)op2.value;

                replacement = (o1 && o2);
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }

            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// ���㡰�����ڡ�����
		/// </summary>
		/// <param name="op1">�������</param>
		/// <param name="op2">�Ҳ�����</param>
		private CSymbol EvaluateNEquals(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            object o1 = null, o2 = null;

            object replacement;
            try
            {
                o1 = op1.value;
                o2 = op2.value;

                bool result = !(o1.Equals(o2));
                replacement = result;
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }

            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// ���㡰�ǡ�������
		/// </summary>
		/// <param name="op1">������</param>
		private CSymbol EvaluateNot(CSymbol op1){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            bool o1 = false;

            object replacement;
            try
            {
                o1 = Convert.ToBoolean(op1.value);

                replacement = (!o1);
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }

            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// ����"��"�����
		/// </summary>
		/// <param name="op1">�������</param>
		/// <param name="op2">�������</param>
		private CSymbol EvaluateOr(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;

            bool o1 = false, o2 = false;

            object replacement;
            try
            {
                try
                {
                    o1 = (bool)op1.value;
                }
                catch
                {

                }

                try
                {
                    o2 = (bool)op2.value;
                }
                catch
                {

                }
                replacement = o1 || o2;
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }
            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// ���㡰���ڡ�
		/// </summary>
		/// <param name="op1">�������</param>
		/// <param name="op2">�Ҳ�����</param>
		private CSymbol EvaluateGreaterThan(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            IComparable o1 = null, o2 = null;

            object replacement;
            try
            {
                o1 = (IComparable)op1.value;
                o2 = (IComparable)op2.value;

                int result;
                result = o1.CompareTo(o2);

                if (result == 1)
                    replacement = true;
                else
                    replacement = false;
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }
            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// ���㡰С�ڡ�������
		/// </summary>
		/// <param name="op1">�������</param>
		/// <param name="op2">�Ҳ�����</param>
		private CSymbol EvaluateLessThan(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            IComparable o1 = null, o2 = null;

            object replacement;
            try
            {
                o1 = (IComparable)op1.value;
                o2 = (IComparable)op2.value;

                int result;
                result = o1.CompareTo(o2);

                if (result == -1)
                    replacement = true;
                else
                    replacement = false;
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }
            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// ���㡰С�ڵ��ڡ�������
		/// </summary>
		/// <param name="op1">�������</param>
		/// <param name="op2">�Ҳ�����</param>
		private CSymbol EvaluateLessThanEqual(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            IComparable o1 = null;
            IComparable o2 = null;

            object replacement;
            try
            {
                o1 = (IComparable)op1.value;
                o2 = (IComparable)op2.value;

                int result;
                result = o1.CompareTo(o2);

                if (result == -1 || result == 0)
                    replacement = true;
                else
                    replacement = false;
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }

            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// ���㡰���ڵ��ڡ�������
		/// </summary>
		/// <param name="op1"></param>
		/// <param name="op2"></param>
		private CSymbol EvaluateGreaterThanEqual(CSymbol op1, CSymbol op2){

            CSymbol op3 = new CSymbol();
            op3.type = CSymbol.Type.Value;
            IComparable o1 = null;
            IComparable o2 = null;

            object replacement;
            try
            {
                o1 = (IComparable)op1.value;
                o2 = (IComparable)op2.value;

                int result;
                result = o1.CompareTo(o2);

                if (result == 1 || result == 0)
                    replacement = true;
                else
                    replacement = false;
            }
            catch
            {
                op3.type = CSymbol.Type.Invalid;
                op3.value = null;
                replacement = op3;
            }

            op3.value = replacement;
            return op3;
		}

		/// <summary>
		/// �Ƚ�higher, lower������ʶ�������ȼ�
		/// ��� higher ���ȼ�����lower������true�����򷵻�false
		/// </summary>
		/// <param name="higher"></param>
		/// <param name="lower"></param>
		private bool DeterminePrecidence(CSymbol higher, CSymbol lower){

            int s1 = Precidence(higher);
            int s2 = Precidence(lower);

            if (s1 > s2)
                return true;
            else
                return false;
		}

		/// <summary>
		/// ���ر�ʶ��s���������ȼ�
		/// </summary>
		/// <param name="s"></param>
		private int Precidence(CSymbol s){

            int result = 0;

            switch (s.name)
            {
                case "*":
                case "/":
                case "%":
                    result = 32;
                    break;
                case "+":
                case "-":
                    result = 16;
                    break;
                case ">":
                case "<":
                case ">=":
                case "<=":
                    result = 8;
                    break;
                case "==":
                case "!=":
                    result = 4;
                    break;
                case "NOT":
                    result = 3;
                    break;
                case "AND":
                    result = 2;
                    break;
                case "XOR":
                case "OR":
                    result = 1;
                    break;
            }           

            return result;
		}

		/// <summary>
		/// ����׺����ʽ������������ȼ�ת��Ϊ��׺����ʽ
		/// ���� a + b * c -> a b c * +
		/// </summary>
		public void InfixToPostfix(){
            //��׺����ʽ���
            m_postfix.Clear();
            //����ջ
            Stack postfixStack = new Stack();
            //����׺����ʽ���б���
            foreach (CSymbol s in m_infix)
            {
                //�����ǰ��ʶ����ֵ���Ǳ���
                //��ֱ�����ӵ���׺����ʽ��
                if (s.type == CSymbol.Type.Value || s.type == CSymbol.Type.Param)
                {                    
                    m_postfix.Add(s);
                }
                //�����ʶ��Ϊ�����
                else if (s.type == CSymbol.Type.Operator)
                {
                    //�����ǰ�������ǰһ������������ȼ�С�������ӵ���׺����ʽ��
                    while (postfixStack.Count > 0 && !DeterminePrecidence(s, (CSymbol)postfixStack.Peek()))
                    {
                        m_postfix.Add((CSymbol)postfixStack.Pop());

                    }
                    //�����ǰ�������ǰһ������������ȼ��������ӵ�����ջ��
                    postfixStack.Push(s);

                }
                else if (s.type == CSymbol.Type.OpenBracket)
                {
                    postfixStack.Push(s);
                }
                else if (s.type == CSymbol.Type.CloseBracket)
                {
                    //�����ǰ��ʶ��Ϊ'(', �򽫸���ջ��')'֮�ϵ�Ԫ�ص�����
                    //�����ӵ���׺����ʽ��
                    while (((CSymbol)postfixStack.Peek()).type != CSymbol.Type.OpenBracket)
                    {
                        m_postfix.Add((CSymbol)postfixStack.Pop());
                    }
                    postfixStack.Pop(); //discard '('
                }
                else
                {
                    throw new Exception("Illegal symbol: " + s.name);
                }
            }
            //��ջ�е�ʣ��Ԫ�ص��������ӵ���׺����ʽ��
            while (postfixStack.Count > 0)
            {
                m_postfix.Add((CSymbol)postfixStack.Pop());
            }            
		}

	}//end CExpressionEvaluator

}//end namespace Flow